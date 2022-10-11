#region License Information (GPL v3)

  /*
     Source code provocatively stolen from ShareX: https://github.com/ShareX/ShareX.
     (Seriously, awesome work over there, I used some of the parts to create an easy
     to use .NET package for everyone.)
     Their License:

     ShareX - A program that allows you to take screenshots and share any file type
     Copyright (c) 2007-2017 ShareX Team
     This program is free software; you can redistribute it and/or
     modify it under the terms of the GNU General Public License
     as published by the Free Software Foundation; either version 2
     of the License, or (at your option) any later version.
     This program is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
     You should have received a copy of the GNU General Public License
     along with this program; if not, write to the Free Software
     Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
     Optionally you can also view the license at <http://www.gnu.org/licenses/>.
 */

  #endregion License Information (GPL v3)


using System;
using System.Runtime.InteropServices;

namespace AnimatedGif {
    /// <summary>
    ///     Summary description for Class1.
    /// </summary>
    public abstract class Quantizer {
        private readonly int _pixelSize;

        /// <summary>
        ///     Flag used to indicate whether a single pass or two passes are needed for quantization.
        /// </summary>
        private readonly bool _singlePass;

        /// <summary>
        ///     Construct the quantizer
        /// </summary>
        /// <param name="singlePass">If true, the quantization only needs to loop through the source pixels once</param>
        /// <remarks>
        ///     If you construct this class with a true value for singlePass, then the code will, when quantizing your image,
        ///     only call the 'QuantizeImage' function. If two passes are required, the code will call 'InitialQuantizeImage'
        ///     and then 'QuantizeImage'.
        /// </remarks>
        protected Quantizer(bool singlePass) {
            _singlePass = singlePass;
            _pixelSize = Marshal.SizeOf(typeof(Color32));
        }

        /// <summary>
        ///     Quantize an image and return the resulting output bitmap
        /// </summary>
        /// <param name="source">The image to quantize</param>
        /// <returns>A quantized version of the image</returns>
        public RawBitmap8 Quantize(RawBitmap32 source) {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // And construct an 8bpp version
            var output = new RawBitmap8(width, height);

            // Call the FirstPass function if not a single pass algorithm.
            // For something like an octree quantizer, this will run through
            // all image pixels, build a data structure, and create a palette.
            if (!_singlePass)
                FirstPass(source, width, height);

            // Then set the color palette on the output bitmap.
            output.Palette = new Color32[256];

            // Then call the second pass which actually does the conversion
            SecondPass(source, output, width, height);

            // Last but not least, return the output bitmap
            return output;
        }

        /// <summary>
        ///     Execute the first pass through the pixels in the image
        /// </summary>
        /// <param name="sourceData">The source data</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        protected virtual void FirstPass(RawBitmap32 sourceData, int width, int height) {
            int index = 0;

            // Loop through each row
            for (int row = 0; row < height; row++) {
                // And loop through each column
                for (int col = 0; col < width; col++, index++) {
                    InitialQuantizePixel(sourceData.Pixels[index]);
                } // Now I have the pixel, call the FirstPassQuantize function...
            }
        }

        /// <summary>
        ///     Execute a second pass through the bitmap
        /// </summary>
        /// <param name="sourceData">The source bitmap, locked into memory</param>
        /// <param name="output">The output bitmap</param>
        /// <param name="width">The width in pixels of the image</param>
        /// <param name="height">The height in pixels of the image</param>
        /// <param name="bounds">The bounding rectangle</param>
        protected virtual void SecondPass(RawBitmap32 sourceData, RawBitmap8 output, int width, int height) {
            int index = 0;

            // And convert the first pixel, so that I have values going into the loop
            var previousPixel = sourceData.Pixels[0];

            byte pixelValue = QuantizePixel(previousPixel);

            // Assign the value of the first pixel
            output.Pixels[index] = pixelValue;

            // Loop through each row
            for (int row = 0; row < height; row++) {
                // Loop through each pixel on this scan line
                for (int col = 0; col < width; col++, index++) {
                    // Check if this is the same as the last pixel. If so use that value
                    // rather than calculating it again. This is an inexpensive optimisation.
                    if (previousPixel != sourceData.Pixels[index]) {
                        // Capture the new previous pixel
                        previousPixel = sourceData.Pixels[index];

                        // Quantize the pixel
                        pixelValue = QuantizePixel(previousPixel);
                    }

                    // And set the pixel in the output
                    output.Pixels[index] = pixelValue;
                }
            }
        }

        /// <summary>
        ///     Override this to process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <remarks>
        ///     This function need only be overridden if your quantize algorithm needs two passes,
        ///     such as an Octree quantizer.
        /// </remarks>
        protected virtual void InitialQuantizePixel(Color32 pixel) { }

        /// <summary>
        ///     Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected abstract byte QuantizePixel(Color32 pixel);

        /// <summary>
        ///     Retrieve the palette for the quantized image
        /// </summary>
        /// <returns>The new color palette</returns>
        protected abstract Color32[] GetPalette();
    }
}
