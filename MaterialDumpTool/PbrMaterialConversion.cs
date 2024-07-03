/**
 * Copyright (c) 2024 chevp
 */

using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

/// <summary>
/// Konvertierung des Ordnerinhalts von PBR-Material-Dateien
/// zu PBR-Dump-Dateien.
/// </summary>
public static class PbrMaterialConversion
{
    /// <summary>
    /// Startet den Konvertierungsvorgang.
    /// </summary>
    /// <param name="dir">Working Directory</param>
    public static void run(String dir)
    {
        createIfNotExist("dump");

        DirectoryInfo place = new DirectoryInfo(dir);

        DirectoryInfo[] Directories = place.GetDirectories();

        foreach (DirectoryInfo i in Directories)
        {
            if (i.Name.Equals("dump"))
                continue;

            Console.WriteLine($"dir={i.Name}");

            try
            {
                createIfNotExist(@$"_dump\{i.Name}");

                convertAllImagesInsideFolder(@$"{dir}\{i.Name}", $@"_dump\{i.Name}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                Console.ReadKey();
            }
        }

        Console.ReadKey();
    }

    /// <summary>
    /// Führt die Konvertierungssequenz aus einem hochauflösenden PBR-Material
    /// zu mehreren PBR-Dump-Dateien aus.
    /// </summary>
    /// <param name="srcDir">Quelldateipfad</param>
    /// <param name="dumpDir">Ausgabepfad für Dump-Dateien</param>
    private static void convertAllImagesInsideFolder(String srcDir, String dumpDir)
    {
        if (!printDirectoryContent(srcDir))
        {
            Console.WriteLine($"{srcDir} has no images");
            return;
        }

        createIfNotExist(@$"{dumpDir}\2048x2048");

        if (!hasDirectoryImages(srcDir))
        {
            copyAllImages(@$"{srcDir}\2048x2048", srcDir);
        }

        copyAllImages(srcDir, @$"{dumpDir}\2048x2048");

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

    /// <summary>
    /// Vereinheitlichung aller Dateinamen innerhalb eines Ordners.
    /// </summary>
    /// <param name="dir">Ordner der zu normalisierenden Dateinamen</param>
    private static void normalizeAllImageNames(string dir)
    {
        DirectoryInfo place = new DirectoryInfo(dir);
        FileInfo[] Files = place.GetFiles();

        foreach (FileInfo i in Files)
        {
            if (i.Name.EndsWith(".png"))
            {
                String newName = normalizedFilename((i.Name));

                if (!newName.Equals(i.Name))
                {
                    File.Move(@$"{dir}\{i.Name}", @$"{dir}\{newName}");
                }
            }
        }
    }

    /// <summary>
    /// Erstellt eine Datei destDir, falls diese noch nicht existiert
    /// </summary>
    /// <param name="destDir">Zieldatei</param>
    /// <param name="delete">Dateilöschung, falls existiert</param>
    private static void createIfNotExist(String destDir, Boolean delete = false)
    {
        try
        {
            if (Directory.Exists(destDir) && delete)
            {
                Directory.Delete(destDir, true);
            }

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());

            Console.ReadKey();

            Environment.Exit(0);
        }
    }
    private static bool printDirectoryContent(String directory)
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

    private static bool hasDirectoryImages(String directory)
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

    /// <summary>
    /// Verschiebung einer Datei von srcPath nach destPath.
    /// </summary>
    /// <param name="srcPath">Quelldateipfad</param>
    /// <param name="destPath">Zieldateipfad</param>
    private static void moveFile(String srcPath, String destPath)
    {
        try
        {
            if (!File.Exists(srcPath))
            {
                // This statement ensures that the file is created,
                // but the handle is not kept.
                using (FileStream fs = File.Create(srcPath)) { }
            }

            // Ensure that the target does not exist.
            if (File.Exists(destPath))
                File.Delete(destPath);

            // Move the file.
            File.Move(srcPath, destPath);
            Console.WriteLine("{0} was moved to {1}.", srcPath, destPath);

            // See if the original exists now.
            if (File.Exists(srcPath))
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

    /// <summary>
    /// Kopie einer Datei von srcPath to destPath.
    /// </summary>
    /// <param name="srcPath">Quelldateiname</param>
    /// <param name="destPath">Zieldateiname</param>
    private static void copyFile(String srcPath, String destPath)
    {
        try
        {
            if (!File.Exists(srcPath))
            {
                // This statement ensures that the file is created,
                // but the handle is not kept.
                using (FileStream fs = File.Create(srcPath)) { }
            }

            // Ensure that the target does not exist.
            if (File.Exists(destPath))
                File.Delete(destPath);

            // Copy the file.
            File.Copy(srcPath, destPath);
            Console.WriteLine("{0} was copied to {1}.", srcPath, destPath);

            // See if the original exists now.
            if (!File.Exists(srcPath))
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

    /// <summary>
    /// Vereinheitlicht unterschiedliche Dateinamen.
    /// </summary>
    /// <param name="srcFilename">Quelldateiname</param>
    /// <returns>Normalisierter Dateiname</returns>
    private static string normalizedFilename(String srcFilename)
    {
        string name = srcFilename.ToLower();

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

        if (!srcFilename.Equals(name))
        {
            Console.WriteLine($"Filename changed old: {srcFilename} new: {name}");
        }

        return name;
    }

    /// <summary>
    /// Verschiebung aller Bilddateien von srcPath nach destPath.
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="destPath"></param>
    private static void moveAllImages(String srcPath, String destPath)
    {
        DirectoryInfo place = new DirectoryInfo(srcPath);

        FileInfo[] Files = place.GetFiles();

        foreach (FileInfo i in Files)
        {
            Console.WriteLine($"src={i.Name} dest={destPath}");

            if (i.Name.EndsWith(".png"))
            {
                moveFile(@$"{srcPath}\{i.Name}", @$"{destPath}\{normalizedFilename(i.Name)}");
            }
        }
    }

    private static void copyAllImages(String src, String dest)
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

    /// <summary>
    /// Konvertiert den Dateiinhalt in ein byte[].
    /// </summary>
    private static byte[] fileToByteArray(String fileName)
    {
        byte[] buff = null;

        FileStream fs = new FileStream(fileName,
            FileMode.Open, FileAccess.Read);

        BinaryReader br = new BinaryReader(fs);

        long numBytes = new FileInfo(fileName).Length;

        buff = br.ReadBytes((int)numBytes);

        return buff;
    }

    /// <summary>
    /// Formatänderung einer Bilddatei.
    /// </summary>
    private static void resize(String inPath, String outPath, Int32 width, Int32 height)
    {
        using (Image image = Image.Load(fileToByteArray(inPath)))
        {
            image.Mutate(x => x.Resize(width, height));

            image.Save(outPath);
        }
    }

    /// <summary>
    /// Formatänderung aller Bilddateien eines Ordners.
    /// </summary>
    private static void resizeImages(String srcFolder, String destFolder, Int32 pixels)
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
}