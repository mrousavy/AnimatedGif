# Animated GIF
**AnimatedGif** is a high performance .NET library for **reading and creating animated GIFs**, inspired by the lack of features from the *System.Windows.Media.GifBitmapEncoder*

# How to use

### Add to your Project
* Via NuGet:

Type `Install-Package AnimatedGif` in Package Manager Console or download manually [on NuGet](http://www.nuget.org/packages/AnimatedGif/)

* Manually: 

Download or clone this Project and compile on your own and import `AnimatedGif/bin/Release/AnimatedGif.dll`

### Creating a GIF
```c#
//Create new Animated GIF Creator with Path to C:\awesomegif.gif and 33ms delay between frames (=30 fps)
using (AnimatedGifCreator gifCreator = AnimatedGif.Create("C:\\awesomegif.gif", 33)) {
    //Enumerate through a List<System.Drawing.Image> or List<System.Drawing.Bitmap> for example
    foreach (Image img in MyImagesList) {
        using (img) {
            //Add the image to gifEncoder with default Quality
            gifCreator.AddFrame(img, GIFQuality.Default);
        } //Image disposed
    }
} // gifCreator.Finish and gifCreator.Dispose is called here
```

### Reading a GIF
```c#
using (GifBitmapDecoder gifDecoder = AnimatedGif.Load("C:\\awesomegif.gif")) {
    // Do stuff
}
```


### Contributing

1. [Fork this Project](https://github.com/mrousavy/AnimatedGif/fork)
2. Change stuff on your Forked repo
3. [Create a pull request](https://github.com/mrousavy/AnimatedGif/compare)
