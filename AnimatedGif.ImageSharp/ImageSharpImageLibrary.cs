using System;
using System.IO;

using SixLabors.ImageSharp;

namespace AnimatedGif.ImageSharp
{
    public class ImageSharpImageLibrary : ImageLibrary
    {
        public override RawBitmap LoadImage(string path)
        {
            // Load the image
            var image = Image.Load(path);

            return BitmapConverter.Convert(image);
        }

        public override void SaveGif(Stream stream, RawBitmap img, GifQuality quality) {
            // ImageSharp only supports implicit GIF creation with 256-colour palettes.
            if ((quality != GifQuality.Bit8) && (quality != GifQuality.Default))
                throw new NotSupportedException("ImageSharp only supports GifQuality.Bit8, which can be selected with GifQuality.Default");

            var image = BitmapConverter.Convert(img);

            image.SaveAsGif(stream);
        }
    }
}
