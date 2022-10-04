using System;

using SixLabors.ImageSharp;

namespace AnimatedGif.ImageSharp {
    public static class AnimatedGifCreatorExtensions {
        public static void AddFrame(this AnimatedGifCreator @this, Image image, int delay = -1, GifQuality quality = GifQuality.Default) {
            @this.AddFrame(
                BitmapConverter.Convert(image),
                delay,
                quality);
        }
    }
}