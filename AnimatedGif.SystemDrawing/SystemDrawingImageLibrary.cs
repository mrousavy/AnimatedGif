using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AnimatedGif.SystemDrawing
{
    public class SystemDrawingImageLibrary : ImageLibrary
    {
        public override RawBitmap LoadImage(string path)
        {
            // Load the image
            var image = Image.FromFile(path);

            // Determine which type of RawBitmap we are loading
            PixelFormat loadFormat;

            switch (image.PixelFormat) {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    loadFormat = PixelFormat.Format8bppIndexed;
                    break;
                default:
                    loadFormat = PixelFormat.Format32bppArgb;
                    break;
            }

            // Convert the Image to a Bitmap
            using (var bitmap = new Bitmap(image.Width, image.Height, loadFormat)) {
                using (var graphics = Graphics.FromImage(bitmap)) {
                    graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                }

                // Lock the raw bitmap data
                BitmapData bitmapData = null;

                try {
                    bitmapData = bitmap.LockBits(
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);

                    // Copy the image data into an appropriate RawBitmap
                    if (bitmap.PixelFormat == PixelFormat.Format32bppArgb) {
                        // Copy into a RawBitmap32
                        var result = new RawBitmap32();

                        result.Width = bitmap.Width;
                        result.Height = bitmap.Height;

                        result.Pixels = new Color32[result.Width * result.Height];

                        int o = 0;

                        for (int y = 0; y < bitmap.Height; y++) {
                            IntPtr scan = bitmapData.Scan0 + y * bitmapData.Stride;

                            for (int x = 0; x < bitmap.Width; x++) {
                                result.Pixels[o++].ARGB = Marshal.ReadInt32(scan);

                                scan += 4;
                            }
                        }

                        return result;
                    }
                    else {
                        // Copy into a RawBitmap8
                        var result = new RawBitmap8();

                        result.Width = bitmap.Width;
                        result.Height = bitmap.Height;

                        result.Pixels = new byte[result.Width * result.Height];

                        int o = 0;

                        for (int y = 0; y < bitmap.Height; y++) {
                            IntPtr scan = bitmapData.Scan0 + y * bitmapData.Stride;

                            for (int x = 0; x < bitmap.Width; x++) {
                                result.Pixels[o++] = Marshal.ReadByte(scan);

                                scan += 1;
                            }
                        }

                        var paletteEntries = bitmap.Palette.Entries;

                        result.Palette = new Color32[paletteEntries.Length];

                        for (int i = 0; i < result.Palette.Length; i++)
                            result.Palette[i] = Color32.FromArgb(paletteEntries[i].ToArgb());

                        return result;
                    }
                }
                finally {
                    // Ensure that the locked bitmap data is released
                    if (bitmapData != null)
                        bitmap.UnlockBits(bitmapData);
                }
            }
        }

        public override void SaveGif(Stream stream, RawBitmap img, GifQuality quality) {
            if (quality == GifQuality.Default) {
                var systemDrawingImage = BitmapConverter.Convert(img);

                systemDrawingImage.Save(stream, ImageFormat.Gif);
            }
            else {
                Quantizer quantizer;
                int maxColors;
                switch (quality) {
                    case GifQuality.Grayscale:
                        quantizer = new GrayscaleQuantizer();
                        maxColors = 0;
                        break;
                    case GifQuality.Bit4:
                        quantizer = new OctreeQuantizer(15, 4);
                        maxColors = 16;
                        break;
                    case GifQuality.Default:
                    case GifQuality.Bit8:
                    default:
                        quantizer = new OctreeQuantizer(255, 4);
                        maxColors = 256;
                        break;
                }

                RawBitmap8 quantized;

                if ((img is RawBitmap8 palettized) && (palettized.Palette.Length <= maxColors)) {
                    quantized = palettized;
                }
                else {
                    if (!(img is RawBitmap32 img32bpp)) {
                        img32bpp = img.ToRawBitmap32();
                    }

                    quantized = quantizer.Quantize(img32bpp);
                }

                using (var quantizedBitmap = BitmapConverter.Convert(quantized)) {
                    quantizedBitmap.Save(stream, ImageFormat.Gif);
                }
            }
        }
    }
}
