# PBR-Material-Dump-Tool

CSharp program for converting PBR materials into dump files.

## Quick Start

```
START MaterialDumpTool.exe --dir PATH_TO_WORKSPACE
```

## Backstory

Large PBR materials with a resolution of 2024x2024 have longer loading times for Vulkan rendering than small dump files.

## Prerequisites

Download PBR materials for conversion.

|Name|Example|
|-|-|
|albedo|`pbrmat1_albedo.png`|
|ao|`pbrmat1_ao.png`|
|height|`pbrmat1_height.png`|
|normal-ogl|`pbrmat1_normal-ogl.png`|
|preview|`pbrmat1_preview.png`|
|roughness|`pbrmat1_roughness.png`|

## Getting Started

Place a list of PBR materials in the workspace.

### Initial Folder Structure

├── pbrmat1
│   ├── pbrmat1_albedo.png
│   ├── pbrmat1_ao.png
│   ├── pbrmat1_height.png
│   ├── pbrmat1_normal-ogl.png
│   ├── pbrmat1_preview.png
│   └── pbrmat1_roughness.png
├── ...
├── pbrmatN

### Start File Conversion

```
START MaterialDumpTool.exe --dir PATH_TO_WORKSPACE
```

### Folder Structure With Converted Files

├── dump
│   ├── pbrmat1
│       ├── 8x8
│           ├── pbrmat1_albedo.png
│           ├── pbrmat1_ao.png
│           ├── pbrmat1_height.png
│           ├── pbrmat1_metallic.png
│           ├── pbrmat1_normal-ogl.png
│           └── pbrmat1_roughness.png
│        ├── 16x16
│           ├── ...
│        ├── 32x32
│           ├── ...
│        ├── 64x64
│           ├── ...
│        ├── 128x128
│           ├── ...
│        ├── 256x256
│           ├── ...
│        ├── 1024x1024
│           ├── ...
│        ├── 2024x2024
│           ├── ...
│   └── ...
│   └── pbrmatN

## Source-Examples for PBR-Materials

- https://www.texturecan.com/

## Open Issues

- [ ] The naming conventions for ambient, occlusion, albedo and metallic roughness files are inconsistent.
- [ ] Single file too large. Should be split into separate files.