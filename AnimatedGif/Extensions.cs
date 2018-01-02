using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AnimatedGif {
    public static class Extensions {
        public static void SaveGif(this Image img, Stream stream, GifQuality quality) {
            if (quality == GifQuality.Default) {
                img.Save(stream, ImageFormat.Gif);
            } else {
                Quantizer quantizer;
                switch (quality) {
                    case GifQuality.Grayscale:
                        quantizer = new GrayscaleQuantizer();
                        break;
                    case GifQuality.Bit4:
                        quantizer = new OctreeQuantizer(15, 4);
                        break;
                    case GifQuality.Default:
                    case GifQuality.Bit8:
                    default:
                        quantizer = new OctreeQuantizer(255, 4);
                        break;
                }

                using (var quantized = quantizer.Quantize(img)) {
                    quantized.Save(stream, ImageFormat.Gif);
                }
            }
        }

        public static void Write(this FileStream stream, byte[] array) {
            stream.Write(array, 0, array.Length);
        }
    }
}