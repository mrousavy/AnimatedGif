<p align="center">
    <img align="right" src="https://raw.githubusercontent.com/mrousavy/AnimatedGif/master/AnimatedGif/Icon.ico" alt="Animated GIF Icon" height=100>
    <h1 align="left">Animated GIF</h1>
</p>

**AnimatedGif** is a high performance .NET library for **reading and creating animated GIFs**, inspired by [ShareX](https://github.com/ShareX/ShareX). It replaces the default `System.Windows.Media.GifBitmapEncoder` to create GIFs from .NET more easily.

[![NuGet](https://img.shields.io/nuget/dt/AnimatedGif.svg)](https://www.nuget.org/packages/AnimatedGif/)

<a href='https://ko-fi.com/F1F8CLXG' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi2.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

# Back Ends

**AnimatedGif** supports multiple back-ends to perform the actual image compression. Each back end is provided in a separate NuGet package. The back end is selected by passing a corresponding `ImageLibrary` implementation into the `GifEngine` constructor.

## Back End: System.Drawing

The integrated System.Drawing library in .NET can be used for compression. This library, provided by Microsoft, is based on [GDI+](https://learn.microsoft.com/en-us/windows/win32/gdiplus/-gdiplus-gdi-start), and is supported only on Windows.

To use System.Drawing, install the `AnimatedGif.SystemDrawing` NuGet package, and then pass an instance of `SystemDrawingImageLibrary` into the `GifEngine` constructor.

## Back End: SixLabors.ImageSharp

An image library called [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) can be used for compression. This third-party library is cross-platform and can be used on any platform where AnimatedGif can run.

To use ImageSharp, install the `AnimatedGif.ImageSharp` NuGet package, and then pass an instance of `ImageSharpImageLibrary` into the `GifEngine` constructor.

ImageSharp does not directly support palettized images. It quantizes the image to an 8-bit palette at the moment of GIF compression. It is not presently possible to select 1bpp or 4bpp with ImageSharp. As such, only `GifQuality.Bit8` can be used with ImageSharp. If other `GifQuality` values are supplied, `ImageSharpImageLibrary` will throw `NotSupportedException`.

# How to use

## Add to your Project

```nuget
Install-Package AnimatedGif
```

> or download manually [on NuGet](http://www.nuget.org/packages/AnimatedGif/)

### Image Library

```nuget
Install-Package AnimatedGif.SystemDrawing
```

or

```nuget
Install-Package AnimatedGif.ImageSharp
```

## Creating a GIF

Create a GIF with the filename `"mygif.gif"` and a `33`ms delay between frames (~30fps). Use `16`ms for 60fps and so on.

```cs
var gifEngine = new GifEngine(new SystemDrawingImageLibrary()); // or ImageSharpImageLibrary,
                                                                // or any supported implementation
using (var gif = gifEngine.Create("mygif.gif", 33))
{
    var img = Image.FromFile("myimage.png");
    gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
}
```

> If you don't want to write to a file directly, the `GifEngine.CreateGif` method also has an overload that accepts a `Stream`.

## Reading a GIF

At the moment there's only a GIF Creator. Create a pull request if you want to create a GIF Reader in this project!

## Contributing

1. [Fork this Project](https://github.com/mrousavy/AnimatedGif/fork)
2. Change stuff on your Forked repo
3. [Create a pull request](https://github.com/mrousavy/AnimatedGif/compare)

