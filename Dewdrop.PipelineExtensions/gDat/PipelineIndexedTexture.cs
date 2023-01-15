using Dewdrop.Utilities;
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

        public int height;
        public int width;
        public byte[] img;
        private int[] paletteColors;
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

            /*for (int i = 0; i < palette.Length; i++)
            {
                palette[i] = ColorHelper.CreateFromInteger(palettes[i / palSize][i % palSize]);
            }*/

            for (uint allPalettes = 0; allPalettes < this.totalPals; allPalettes++)
            {
                uint num3 = 0;
                while (num3 < palettes[allPalettes].Length)
                {
                    palette[allPalettes *this.palSize + num3] = ColorHelper.CreateFromInteger(palettes[allPalettes][num3]);
                    num3++;
                }
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
