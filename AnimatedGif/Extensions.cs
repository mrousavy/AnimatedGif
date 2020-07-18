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


using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AnimatedGif {
    public static class Extensions {
        public static void SaveGif(this Image img, Stream stream, GifQuality quality) {
            if (quality == GifQuality.Default) {
                img.Save(stream, ImageFormat.Gif);
            } else {
                Quantizer quantizer;
                switch (quality) {
                    case GifQuality.Grayscale:
                        quantizer = new GrayscaleQuantizer();
                        break;
                    case GifQuality.Bit4:
                        quantizer = new OctreeQuantizer(15, 4);
                        break;
                    case GifQuality.Default:
                    case GifQuality.Bit8:
                    default:
                        quantizer = new OctreeQuantizer(255, 4);
                        break;
                }

                using (var quantized = quantizer.Quantize(img)) {
                    quantized.Save(stream, ImageFormat.Gif);
                }
            }
        }

        public static void Write(this FileStream stream, byte[] array) {
            stream.Write(array, 0, array.Length);
        }
    }
}
