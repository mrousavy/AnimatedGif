namespace AnimatedGif
{
    public abstract class RawBitmap
    {
        public int Width;
        public int Height;

        public abstract RawBitmap32 ToRawBitmap32();
    }
}
