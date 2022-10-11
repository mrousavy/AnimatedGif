using System;
using System.Runtime.InteropServices;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AnimatedGif.ImageSharp {
    public class BitmapConverter {
        public static RawBitmap32 Convert(Image source) {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // First off ensure that we have the image in 32bpp pixel format
            if (!(source is Image<Rgba32> source32bpp))
                source32bpp = new Image<Rgba32>(width, height);

            try {
                if (!ReferenceEquals(source32bpp, source)) {
                    source32bpp.Mutate(
                        context => context.DrawImage(source, new GraphicsOptions()));
                }

                var rawBitmap = new RawBitmap32();

                rawBitmap.Width = width;
                rawBitmap.Height = height;

                rawBitmap.Pixels = new Color32[width * height];

                source32bpp.ProcessPixelRows(
                    accessor => {
                        int o = 0;

                        for (int y = 0; y < accessor.Height; y++) {
                            var pixelRow = accessor.GetRowSpan(y);

                            for (int x = 0; x < accessor.Width; x++) {
                                rawBitmap.Pixels[o] = Color32.FromArgb(pixelRow[x].PackedValue);
                                o++;
                            }
                        }
                    });

                return rawBitmap;
            }
            finally {
                if (!ReferenceEquals(source32bpp, source))
                    source32bpp.Dispose();
            }
        }

        public static Image Convert(RawBitmap rawBitmap) {
            if (rawBitmap is RawBitmap32 rawBitmap32)
                return Convert(rawBitmap32);
            if (rawBitmap is RawBitmap8 rawBitmap8)
                return Convert(rawBitmap8);

            throw new ArgumentException();
        }

        public static Image Convert(RawBitmap32 rawBitmap) {
            var bitmap = new Image<Rgba32>(rawBitmap.Width, rawBitmap.Height);

            bitmap.ProcessPixelRows(
                accessor => {
                    int o = 0;

                    for (int y = 0; y < accessor.Height; y++) {
                        var pixelRow = accessor.GetRowSpan(y);

                        for (int x = 0; x < accessor.Width; x++) {
                            pixelRow[x].PackedValue = unchecked((uint)rawBitmap.Pixels[o].ARGB);
                            o++;
                        }
                    }
                });

            return bitmap;
        }
    }
}
