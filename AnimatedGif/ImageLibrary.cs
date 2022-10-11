using System;
using System.IO;

namespace AnimatedGif
{
    public abstract class ImageLibrary
    {
        public abstract RawBitmap LoadImage(string path);

        public abstract void SaveGif(Stream target, RawBitmap img, GifQuality quality);
    }
}
