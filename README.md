<p align="center">
    <img align="right" src="https://raw.githubusercontent.com/mrousavy/AnimatedGif/master/AnimatedGif/Icon.ico" alt="Animated GIF Icon" height=100>
    <h1 align="left">Animated GIF</h1>
</p>

**AnimatedGif** is a high performance .NET library for **reading and creating animated GIFs**, inspired by the lack of features from the *System.Windows.Media.GifBitmapEncoder*

[![NuGet](https://img.shields.io/nuget/dt/AnimatedGif.svg)](https://www.nuget.org/packages/AnimatedGif/)

# How to use

## Add to your Project

```nuget
Install-Package AnimatedGif
```

> or download manually [on NuGet](http://www.nuget.org/packages/AnimatedGif/)


## Creating a GIF

Create a GIF with the filename `"mygif.gif"` and a `33`ms delay between frames (~30fps). Use `16`ms for 60fps and so on.

```cs
using (var gif = AnimatedGif.Create("mygif.gif", 33))
{
    var img = Image.FromFile("myimage.png");
    gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
}
```

> If you don't want to write to a File, the first parameter can also be a `Stream`.

## Reading a GIF

At the moment there's only a GIF Creator. Create a pull request if you want to create a GIF Reader in this project!

## Contributing

1. [Fork this Project](https://github.com/mrousavy/AnimatedGif/fork)
2. Change stuff on your Forked repo
3. [Create a pull request](https://github.com/mrousavy/AnimatedGif/compare)

<a href='https://ko-fi.com/F1F8CLXG' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi2.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>
