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
                palette[i] = ColorHelper.CreateFromInteger(palettes[i / palSize][i % palSize]);
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
