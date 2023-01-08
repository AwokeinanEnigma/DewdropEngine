using DewdropEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.PipelineReaders
{
    /// <summary>
    /// Provides a reader of raw sprite sheet content from the content pipeline.
    /// </summary>
    public class IndexedTexture2DTestReader : ContentTypeReader<IndexedTexture2DTest>
    {
        /// <inheritdoc />s
        /// 
        /// <summary>
        /// Gets a color from an unsigned integer
        /// </summary>
        /// <param name="color">The unsigned integer to get the color from.</param>
        /// <returns>Returns the color from the unsigned integer</returns>
        public Color FromInt(uint color)
        {
            // inherited from carbine
            // i don't know how this code works, and frankly, i don't want to know. 
            byte alpha = (byte)(color >> 24);
            return new Color((byte)(color >> 16), (byte)(color >> 8), (byte)color, alpha);
        }

        protected override IndexedTexture2DTest Read(ContentReader input, IndexedTexture2DTest existingInstance)
        {
            int imageWidth = input.ReadInt32(); //1: output.Write(value.Asset.width);;
            int imageHeight = input.ReadInt32(); //2: output.Write(value.Asset.height);

            int totalPalettes = input.ReadInt32(); //3: output.Write(value.Asset.totalPals);
            int paletteSize = input.ReadInt32(); //4: output.Write(value.Asset.palSize);

            int colorCount = input.ReadInt32(); //5: output.Write(value.Asset.image.Length);

            // Decompress the single integer back into an array of Microsoft.XNA.Framework.Color
            byte[] image = input.ReadBytes(colorCount);
            Color[] coloredImage = new Color[imageWidth * imageHeight];
            for (int i = 0; i < image.Length; i++)
            {
                coloredImage[i].A = byte.MaxValue;
                coloredImage[i].R = image[i];
                coloredImage[i].G = image[i];
                coloredImage[i].B = image[i];
            }
            Console.WriteLine($"img: {colorCount}");

            int paletteColorCount = input.ReadInt32(); //7: output.Write(value.Asset.palette.Length);

            Color[] decompressedPalettes = new Color[paletteColorCount];
            for (int i = 0; i < paletteColorCount; i++)
            {
                decompressedPalettes[i] = FromInt((uint)input.ReadInt32()); //8: output.Write(paletteColor);
            }
            Console.WriteLine($"pal: {paletteColorCount}");

            int spriteDefinitionCount = input.ReadInt32(); //9: output.Write(value.Asset.definitions.Count);
            string defaultSpriteDefinitionName = input.ReadString(); //10: output.Write(value.Asset.defaultDefinition.Name);
            Console.WriteLine($"dname:{defaultSpriteDefinitionName}");

            Dictionary<int, SpriteDefinition> spriteDefinitions = new Dictionary<int, SpriteDefinition>();
            SpriteDefinition defaultSpriteDefinition = null;

            for (int i = 0; i < spriteDefinitionCount; i++)
            {
                string name = input.ReadString(); //11: output.Write(def.Name);

                float coordinatesX = input.ReadSingle(); //12: output.Write(def.Coords.X);
                float coordinatesY = input.ReadSingle(); //13: output.Write(def.Coords.Y);

                float boundsX = input.ReadSingle(); //14: output.Write(def.Bounds.X);
                float boundsY = input.ReadSingle(); //15: output.Write(def.Bounds.Y);

                float originX = input.ReadSingle(); //16: output.Write(def.Origin.X);
                float originY = input.ReadSingle(); //17: output.Write(def.Origin.Y);

                int frames = input.ReadInt32(); //18:  output.Write(def.Frames);

                float speed = input.ReadSingle(); //19: output.Write(def.Speeds[0]);

                bool flipX = input.ReadBoolean(); //20: output.Write(def.FlipX);
                bool flipY = input.ReadBoolean(); //21: output.Write(def.FlipY);

                int mode = input.ReadInt32(); //22: output.Write((int)def.Mode);

                //hash the string!
                SpriteDefinition newDefinition = new SpriteDefinition(name, new Vector2(coordinatesX, coordinatesY), new Vector2(boundsX, boundsY), new Vector2(originX, originY), frames, new float[1] { speed }, flipX, flipY, mode, Array.Empty<int>());
                if (defaultSpriteDefinition == null && name == defaultSpriteDefinitionName)
                {
                    defaultSpriteDefinition = newDefinition;
                }
                spriteDefinitions.Add(name.GetHashCode(), newDefinition);

            }
            return new IndexedTexture2DTest(imageWidth, imageHeight, paletteSize, totalPalettes, coloredImage, decompressedPalettes, spriteDefinitions, defaultSpriteDefinition);
        }
    }
}
