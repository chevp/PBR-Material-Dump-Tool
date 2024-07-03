/**
 * Copyright (c) 2024 chevp
 */

using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

Console.WriteLine("MaterialDumpTool starts...");

string currDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

for (int i = 0; i < args.Length - 1; i++)
{
    if (args[i].Equals("--dir"))
    {
        currDirectory = args[i + 1];
    }
}

createIfNotExist("dump");

DirectoryInfo place = new DirectoryInfo(currDirectory);
DirectoryInfo[] Directories = place.GetDirectories();

foreach (DirectoryInfo i in Directories)
{
    if (i.Name.Equals("dump"))
        continue;

    Console.WriteLine($"dir={i.Name}");

    try
    {
        createIfNotExist(@$"_dump\{i.Name}");

        doStuff(@$"{currDirectory}\{i.Name}", $@"_dump\{i.Name}");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
        Console.ReadKey();
    }    
}

Console.ReadKey();


static void doStuff(string dir, string dumpDir) {

    if (!printDirectoryContent(dir))
    {
        Console.WriteLine($"{dir} has no images");
        return;
    }

    createIfNotExist(@$"{dumpDir}\2048x2048");

    if (!hasDirectoryImages(dir))
    {
        copyAllImages(@$"{dir}\2048x2048", dir);
    }

    copyAllImages(dir, @$"{dumpDir}\2048x2048");

    normalizeAllImageNames(@$"{dumpDir}\2048x2048");

    createIfNotExist(@$"{dumpDir}\8x8", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\8x8", 8);

    createIfNotExist(@$"{dumpDir}\16x16", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\16x16", 16);

    createIfNotExist(@$"{dumpDir}\32x32", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\32x32", 32);

    createIfNotExist(@$"{dumpDir}\64x64", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\64x64", 64);

    createIfNotExist(@$"{dumpDir}\128x128", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\128x128", 128);

    createIfNotExist(@$"{dumpDir}\256x256", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\256x256", 256);

    createIfNotExist(@$"{dumpDir}\512x512", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\512x512", 512);

    createIfNotExist(@$"{dumpDir}\1024x1024", true);
    resizeImages(@$"{dumpDir}\2048x2048", @$"{dumpDir}\1024x1024", 1024);

}
static void normalizeAllImageNames(string dir)
{
    DirectoryInfo place = new DirectoryInfo(dir);
    FileInfo[] Files = place.GetFiles();

    foreach (FileInfo i in Files)
    {
        if (i.Name.EndsWith(".png"))
        {
            string newName = normalizedFilename((i.Name));

            if (!newName.Equals(i.Name))
            {
                File.Move(@$"{dir}\{i.Name}", @$"{dir}\{newName}");
            }
        }
    }
}
static void createIfNotExist(string a, bool delete = false)
{
    try
    { 
        if (Directory.Exists(a) && delete)
        {
            Directory.Delete(a, true);
        }

        if (!Directory.Exists(a))
        {
            Directory.CreateDirectory(a);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("The process failed: {0}", e.ToString());

        Console.ReadKey();

        Environment.Exit(0);
    }
}
static bool printDirectoryContent(string directory)
{
    Console.WriteLine("List of all Files:");
    DirectoryInfo place = new DirectoryInfo(directory);
    FileInfo[] Files = place.GetFiles();
    bool hasImage = true;
    foreach (FileInfo i in Files)
    {
        if (i.Name.Contains("review"))
            continue;

        Console.WriteLine(i.Name);

        if (i.Name.EndsWith(".ignore"))
            hasImage = false;
    }

    return hasImage;
}

static bool hasDirectoryImages(string directory)
{
    Console.WriteLine("List of all Files:");
    DirectoryInfo place = new DirectoryInfo(directory);
    FileInfo[] Files = place.GetFiles();
    bool hasImage = false;
    foreach (FileInfo i in Files)
    {
        if (i.Name.Contains("review"))
            continue;

        if (i.Name.EndsWith(".png"))
            return true;
    }

    return hasImage;
}

static void moveFile(string a, string b)
{
    try
    {
        if (!File.Exists(a))
        {
            // This statement ensures that the file is created,
            // but the handle is not kept.
            using (FileStream fs = File.Create(a)) { }
        }

        // Ensure that the target does not exist.
        if (File.Exists(b))
            File.Delete(b);

        // Move the file.
        File.Move(a, b);
        Console.WriteLine("{0} was moved to {1}.", a, b);

        // See if the original exists now.
        if (File.Exists(a))
        {
            Console.WriteLine("The original file still exists, which is unexpected.");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("The process failed: {0}", e.ToString());

        Console.ReadKey();

        Environment.Exit(0);
    }
}

static void copyFile(string a, string b)
{
    try
    {
        if (!File.Exists(a))
        {
            // This statement ensures that the file is created,
            // but the handle is not kept.
            using (FileStream fs = File.Create(a)) { }
        }

        // Ensure that the target does not exist.
        if (File.Exists(b))
            File.Delete(b);

        // Copy the file.
        File.Copy(a, b);
        Console.WriteLine("{0} was copied to {1}.", a, b);

        // See if the original exists now.
        if (!File.Exists(a))
        {
            Console.WriteLine("The original file not exists, which is unexpected.");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("The process failed: {0}", e.ToString());

        Console.ReadKey();

        Environment.Exit(0);
    }
}
static string normalizedFilename(string curFilename)
{
    string name = curFilename.ToLower();
    if (name.EndsWith("roughnessmetalness.png"))
        name = name.Replace("roughnessmetalness", "metalRoughness");
    if (name.EndsWith("-ao.png"))
        name = name.Replace("-ao.png", "_ao.png");
    if (name.EndsWith("-albedo.png"))
        name = name.Replace("-albedo.png", "_albedo.png");
    if (name.EndsWith("-height.png"))
        name = name.Replace("-height.png", "_height.png");
    if (name.EndsWith("-normal-ogl.png"))
        name = name.Replace("-normal-ogl.png", "_normal-ogl.png");
    if (name.EndsWith("-normal.png"))
        name = name.Replace("-normal.png", "_normal.png");
    if (name.EndsWith("-metallic.png"))
        name = name.Replace("-metallic.png", "_metallic.png");
    if (name.EndsWith("-roughness.png"))
        name = name.Replace("-roughness.png", "_roughness.png");
    if (name.EndsWith("-metallic.png"))
        name = name.Replace("-metallic.png", "_metallic.png");

    if (!curFilename.Equals(name))
    {
        Console.WriteLine($"Filename changed old: {curFilename} new: {name}");
    }
    
    return name;
}
static void moveAllImages(string src, string dest)
{
    DirectoryInfo place = new DirectoryInfo(src);
    FileInfo[] Files = place.GetFiles();

    foreach (FileInfo i in Files)
    {
        Console.WriteLine($"src={i.Name} dest={dest}");

        if (i.Name.EndsWith(".png"))
        {
            moveFile(@$"{src}\{i.Name}", @$"{dest}\{normalizedFilename(i.Name)}");
        }
    }
}

static void copyAllImages(string src, string dest)
{
    DirectoryInfo place = new DirectoryInfo(src);
    FileInfo[] Files = place.GetFiles();
    foreach (FileInfo i in Files)
    {
        Console.WriteLine($"src={i.Name} dest={dest}");

        if (i.Name.EndsWith(".png"))
        {
            copyFile(@$"{src}\{i.Name}", @$"{dest}\{i.Name}");
        }
    }
}
static byte[] fileToByteArray(string fileName)
{
    byte[] buff = null;
    FileStream fs = new FileStream(fileName,
        FileMode.Open, FileAccess.Read);
    BinaryReader br = new BinaryReader(fs);
    long numBytes = new FileInfo(fileName).Length;
    buff = br.ReadBytes((int)numBytes);
    return buff;
}
static void resize(string inPath, string outPath, int width, int height)
{
    using (Image image = Image.Load(fileToByteArray(inPath)))
    {
        image.Mutate(x => x.Resize(width, height));

        image.Save(outPath);
    }
}

static void resizeImages(string srcFolder, string destFolder, int pixels)
{
    DirectoryInfo place = new DirectoryInfo(srcFolder);
    FileInfo[] Files = place.GetFiles();
    foreach (FileInfo i in Files)
    {
        Console.WriteLine($"src={i.Name}");

        if (i.Name.EndsWith(".png"))
        {
            try
            {
                resize(@$"{srcFolder}\{i.Name}", @$"{destFolder}\{i.Name}", pixels, pixels);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());

                Console.ReadKey();

                Environment.Exit(0);
            }
        }
    }
}