using Microsoft.Xna.Framework;
using System.Globalization;

namespace Dewdrop.Utilities
{
    /// <summary>
    /// Provides methods to manipulate colors
    /// </summary>
    public static class ColorHelper
    {
        public static Color FromHexString(string hexString)
        {
            Color result;
            try
            {
                int color = int.Parse(hexString, NumberStyles.HexNumber);
                result = CreateFromInteger(color);
            }
            catch (Exception)
            {
                result = Color.White;
            }
            return result;
        }

        /// <summary>
        /// Creates a <see cref="Color"/> object from a 32-bit signed integer representation.
        /// </summary>
        /// <param name="color">The 32-bit signed integer representing the color in the format of 0xAARRGGBB</param>
        /// <returns>A new <see cref="Color"/> object created from the specified integer value.</returns>
        public static Color CreateFromInteger(int color) => CreateFromInteger((uint)color);

        /// <summary>
        /// Creates a <see cref="Color"/> object from a 32-bit unsigned integer representation.
        /// </summary>
        /// <param name="color">The 32-bit unsigned integer representing the color in the format of 0xAARRGGBB</param>
        /// <returns>A new <see cref="Color"/> object created from the specified integer value.</returns>
        public static Color CreateFromInteger(uint color)
        {
            // inherited from carbine
            // i don't know how this code works, and frankly, i don't want to know. 
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> object to a 32-bit signed integer representation in the format of 0xAARRGGBB.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> object to be converted.</param>
        /// <returns>A 32-bit signed integer representation of the specified <see cref="Color"/> object.</returns>
        public static int ToARGB(Color color)
        {
            int argb = color.A << 24; // Shift the alpha bits over 24 places
            argb |= color.R << 16; // Or it with the red bits shifted 16 places
            argb |= color.G << 8; // Or it with the green bits shifted 8 places
            argb |= color.B; // Or it with the blue bits
            return argb;
        }
    }
}
