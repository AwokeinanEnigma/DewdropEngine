using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Dewdrop.PipelineExtensions.GDat
{
    public class PipelineIndexedTexture
    {
        public PipelineSpriteDefinition defaultDefinition;
        public List<PipelineSpriteDefinition> definitions;

        public int currentPal;
        public int totalPals;
        public int palSize;

        public Color[] image;
        public Color[] palette;

        /// <summary>
        /// Gets a color from an integer
        /// </summary>
        /// <param name="color">The integer to get the color from.</param>
        /// <returns>Returns the color from the integer</returns>
        public static Color FromInt(int color) => FromInt((uint)color);

        /// <summary>
        /// Gets a color from an unsigned integer
        /// </summary>
        /// <param name="color">The unsigned integer to get the color from.</param>
        /// <returns>Returns the color from the unsigned integer</returns>
        public static Color FromInt(uint color)
        {
            // inherited from carbine
            // i don't know how this code works, and frankly, i don't want to know. 
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        public int height;
        public int width;
        public byte[] img;

        public PipelineIndexedTexture(int cWidth, int[][] palettes, byte[] image, List<PipelineSpriteDefinition> definitions, PipelineSpriteDefinition defaultDefinition)
        {
            totalPals = palettes.Length;
            palSize = palettes[0].Length;
            height = image.Length / cWidth;
            width = cWidth;
            // Console.WriteLine(BitConverter.ToString(image));
            img = image;
            palette = new Color[palSize * totalPals];
            this.image = new Color[width * height];

            for (int i = 0; i < palette.Length; i++)
            {
                palette[i] = FromInt(palettes[i / palSize][i % palSize]);
            }

            for (int i = 0; i < this.image.Length; i++)
            {
                this.image[i].A = byte.MaxValue;
                this.image[i].R = image[i];
                this.image[i].G = image[i];
                this.image[i].B = image[i];
            }
            this.definitions = definitions;
            this.defaultDefinition = defaultDefinition;
        }


        public ICollection<PipelineSpriteDefinition> GetSpriteDefinitions()
        {
            return definitions;
        }

        public PipelineSpriteDefinition GetDefaultSpriteDefinition()
        {
            return defaultDefinition;
        }

    }
}
