using System;
using System.Drawing;

namespace AnimatedGif.SystemDrawing {
    public static class AnimatedGifCreatorExtensions {
        public static void AddFrame(this AnimatedGifCreator @this, Image image, int delay = -1, GifQuality quality = GifQuality.Default) {
            @this.AddFrame(
                BitmapConverter.Convert(image),
                delay,
                quality);
        }
    }
}