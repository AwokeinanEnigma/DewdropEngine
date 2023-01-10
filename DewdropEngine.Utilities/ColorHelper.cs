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
        /// Gets a color from an integer
        /// </summary>
        /// <param name="color">The integer to get the color from.</param>
        /// <returns>Returns the color from the integer</returns>
        public static Color CreateFromInteger(int color) => ColorHelper.CreateFromInteger((uint)color);

        /// <summary>
        /// Gets a color from an unsigned integer
        /// </summary>
        /// <param name="color">The unsigned integer to get the color from.</param>
        /// <returns>Returns the color from the unsigned integer</returns>
        public static Color CreateFromInteger(uint color)
        {
            // inherited from carbine
            // i don't know how this code works, and frankly, i don't want to know. 
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        public static int ToInt(Color color)
        {
            int argb = color.A << 24; // Shift the alpha bits over 24 places
            argb |= color.R << 16; // Or it with the red bits shifted 16 places
            argb |= color.G << 8; // Or it with the green bits shifted 8 places
            argb |= color.B; // Or it with the blue bits
            return argb;
        }
    }
}
