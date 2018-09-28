namespace AnimatedGif {
    public static class AnimatedGif {
        /// <summary>
        ///     Create a new Animated GIF
        /// </summary>
        /// <param name="filePath">The Path where the Animated GIF gets saved</param>
        /// <param name="delay">Delay between frames</param>
        /// <param name="repeat">GIF Repeat count (0 meaning forever)</param>
        /// <returns></returns>
        public static AnimatedGifCreator Create(string filePath, int delay, int repeat = 0) {
            return new AnimatedGifCreator(filePath, delay, repeat);
        }
    }
}