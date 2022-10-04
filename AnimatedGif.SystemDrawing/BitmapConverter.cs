using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AnimatedGif.SystemDrawing {
    public class BitmapConverter {
        public static RawBitmap32 Convert(Image source) {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // And construct a rectangle from these dimensions
            var bounds = new Rectangle(0, 0, width, height);

            // First off take a 32bpp copy of the image
            using (var copy = new Bitmap(width, height, PixelFormat.Format32bppArgb)) {
                using (var g = Graphics.FromImage(copy)) {
                    g.PageUnit = GraphicsUnit.Pixel;

                    // Draw the source image onto the copy bitmap,
                    // which will effect a widening as appropriate.
                    g.DrawImage(source, bounds);
                }

                var rawBitmap = new RawBitmap32();

                rawBitmap.Width = width;
                rawBitmap.Height = height;

                rawBitmap.Pixels = new Color32[width * height];

                // Define a pointer to the bitmap data
                BitmapData sourceData = null;

                try {
                    // Get the source image bits and lock into memory
                    sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                    // Copy the raw image data
                    int o = 0;

                    for (int y = 0; y < rawBitmap.Height; y++) {
                        var scan = sourceData.Scan0 + y * sourceData.Stride;

                        for (int x = 0; x < rawBitmap.Width; x++) {
                            rawBitmap.Pixels[o++].ARGB = Marshal.ReadInt32(scan);
                            scan += 4;
                        }
                    }
                }
                finally {
                    // Ensure that the bits are unlocked
                    copy.UnlockBits(sourceData);
                }

                return rawBitmap;
            }
        }

        public static Bitmap Convert(RawBitmap rawBitmap) {
            if (rawBitmap is RawBitmap32 rawBitmap32)
                return Convert(rawBitmap32);
            if (rawBitmap is RawBitmap8 rawBitmap8)
                return Convert(rawBitmap8);

            throw new ArgumentException();
        }

        public static Bitmap Convert(RawBitmap32 rawBitmap) {
            var bitmap = new Bitmap(rawBitmap.Width, rawBitmap.Height, PixelFormat.Format32bppArgb);

            BitmapData bitmapData = null;

            try {
                bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);

                for (int y = 0, o = 0; y < bitmap.Height; y++) {
                    IntPtr scan = bitmapData.Scan0 + y * bitmapData.Stride;

                    for (int x = 0; x < bitmap.Width; x++, o++) {
                        Marshal.WriteInt32(scan, rawBitmap.Pixels[o].ARGB);

                        scan += 4;
                    }
                }
            }
            finally {
                if (bitmapData != null)
                    bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
        }

        private static Bitmap Convertaeu(RawBitmap8 rawBitmap) {
            var bitmap = new Bitmap(rawBitmap.Width, rawBitmap.Height, PixelFormat.Format8bppIndexed);

            BitmapData bitmapData = null;

            try {
                bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format8bppIndexed);

                for (int y = 0; y < bitmap.Height; y++) {
                    Marshal.Copy(
                        rawBitmap.Pixels,
                        y * rawBitmap.Width,
                        bitmapData.Scan0 + y * bitmapData.Stride,
                        rawBitmap.Width);
                }
            }
            finally {
                if (bitmapData != null)
                    bitmap.UnlockBits(bitmapData);
            }

            for (int i = 0; i < rawBitmap.Palette.Length; i++) {
                bitmap.Palette.Entries[i] = Color.FromArgb(rawBitmap.Palette[i].ARGB);
            }

            return bitmap;
        }
    }
}
