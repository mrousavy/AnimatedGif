using System;
using System.Runtime.InteropServices;

namespace AnimatedGif
{
    /// <summary>
    ///     Struct that defines a 32 bpp colour
    /// </summary>
    /// <remarks>
    ///     This struct is used to read data from a 32 bits per pixel image
    ///     in memory, and is ordered in this manner as this is the way that
    ///     the data is layed out in memory
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color32 {
        public static Color32 FromArgb(int argb) {
            return
                new Color32() {
                    ARGB = argb
                };
        }

        public static Color32 FromArgb(uint argb) {
            return
                new Color32() {
                    ARGB = unchecked((int)argb)
                };
        }

        public static Color32 FromArgb(byte red, byte green, byte blue, byte alpha) {
            return
                new Color32()
                {
                    Red = red,
                    Green = green,
                    Blue = blue,
                    Alpha = alpha,
                };
        }

        public static Color32 FromArgb(byte red, byte green, byte blue) {
            return
                new Color32()
                {
                    Red = red,
                    Green = green,
                    Blue = blue,
                    Alpha = 255,
                };
        }

        public static Color32 FromArgb(int red, int green, int blue, int alpha) {
            unchecked {
                return
                    new Color32() {
                        Red = (byte)red,
                        Green = (byte)green,
                        Blue = (byte)blue,
                        Alpha = (byte)alpha,
                    };
            }
        }

        public static Color32 FromArgb(int red, int green, int blue) {
            unchecked {
                return
                    new Color32()
                    {
                        Red = (byte)red,
                        Green = (byte)green,
                        Blue = (byte)blue,
                        Alpha = 255,
                    };
            }
        }

        /// <summary>
        ///     Holds the blue component of the colour
        /// </summary>
        [FieldOffset(0)] public byte Blue;

        /// <summary>
        ///     Holds the green component of the colour
        /// </summary>
        [FieldOffset(1)] public byte Green;

        /// <summary>
        ///     Holds the red component of the colour
        /// </summary>
        [FieldOffset(2)] public byte Red;

        /// <summary>
        ///     Holds the alpha component of the colour
        /// </summary>
        [FieldOffset(3)] public byte Alpha;

        /// <summary>
        ///     Permits the color32 to be treated as an int32
        /// </summary>
        [FieldOffset(0)] public int ARGB;

        public static bool operator ==(Color32 left, Color32 right)
            => left.ARGB == right.ARGB;
        public static bool operator !=(Color32 left, Color32 right)
            => left.ARGB != right.ARGB;

        public override bool Equals(object obj)
            => (obj is Color32 otherColor) && (this == otherColor);
        public override int GetHashCode()
            => ARGB;
    }
}
