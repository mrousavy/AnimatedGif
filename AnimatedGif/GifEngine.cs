using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnimatedGif {
    public class GifEngine {
        ImageLibrary _imageLibrary;

        public GifEngine(ImageLibrary imageLibrary) {
            _imageLibrary = imageLibrary;
        }

        public AnimatedGifCreator CreateGif(string filePath, int delay = 33, int repeat = 0) {
            return new AnimatedGifCreator(filePath, delay, repeat, _imageLibrary);
        }

        public AnimatedGifCreator CreateGif(Stream stream, int delay = 33, int repeat = 0) {
            return new AnimatedGifCreator(stream, delay, repeat, _imageLibrary);
        }
    }
}
