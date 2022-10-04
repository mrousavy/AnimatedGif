using System.Drawing;

using AnimatedGif.SystemDrawing;

namespace AnimatedGif.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gifEngine = new GifEngine(new SystemDrawingImageLibrary());

            // 33ms delay (~30fps)
            using (var gif = gifEngine.CreateGif("gif.gif", 33))
            {
                var img1 = Image.FromFile("img1.png");
                gif.AddFrame(img1, delay: -1, quality: GifQuality.Bit8);
                var img2 = Image.FromFile("img2.png");
                gif.AddFrame(img2, delay: -1, quality: GifQuality.Bit8);
                var img3 = Image.FromFile("img3.png");
                gif.AddFrame(img3, delay: -1, quality: GifQuality.Bit8);
            }
        }
    }
}
