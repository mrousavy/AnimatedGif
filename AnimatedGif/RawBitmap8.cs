namespace AnimatedGif
{
    public class RawBitmap8 : RawBitmap
    {
        public byte[] Pixels;
        public Color32[] Palette;

        public RawBitmap8()
        {
        }

        public RawBitmap8(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override RawBitmap32 ToRawBitmap32() {
            var ret = new RawBitmap32();

            ret.Width = Width;
            ret.Height = Height;

            ret.Pixels = new Color32[Pixels.Length];

            for (int i = 0; i < Pixels.Length; i++)
                ret.Pixels[i] = this.Palette[this.Pixels[i]];

            return ret;
        }
    }
}
