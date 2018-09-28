using System.Drawing;

namespace AnimatedGif.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 33ms delay (~30fps)
            using (var gif = AnimatedGif.Create("gif.gif", 33))
            {
                var img = Image.FromFile("img.png");
                gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
            }
        }
    }
}
