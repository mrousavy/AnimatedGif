using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimatedGif.SystemDrawing {
    public static class QuantizerExtensions {
        public static RawBitmap8 Quantize(this Quantizer quantizer, Image source) {
            var sourceRawBitmap = BitmapConverter.Convert(source);

            return quantizer.Quantize(sourceRawBitmap);
        }
    }
}
