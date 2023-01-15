using Dewdrop.Graphics;
using Dewdrop.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Dewdrop.PipelineReaders
{
    /// <summary>
    /// Provides a reader of raw sprite sheet content from the content pipeline.
    /// </summary>
    public class IndexedTexture2DReader : ContentTypeReader<SpriteSheetTexture>
    {
        /// <inheritdoc />s
        protected override SpriteSheetTexture Read(ContentReader input, SpriteSheetTexture existingInstance)
        {
            int imageWidth = input.ReadInt32(); //1: output.Write(value.Asset.width);;
            int imageHeight = input.ReadInt32(); //2: output.Write(value.Asset.height);

            int totalPalettes = input.ReadInt32(); //3: output.Write(value.Asset.totalPals);
            int paletteSize = input.ReadInt32(); //4: output.Write(value.Asset.palSize);

            int colorCount = input.ReadInt32(); //5: output.Write(value.Asset.image.Length);

            // Decompress the single integer back into an array of Microsoft.XNA.Framework.Color
            byte[] image = input.ReadBytes(colorCount);
            //byte[] image = Decompress(preImg);
            Color[] coloredImage = new Color[imageWidth * imageHeight];
            // Use a local variable for the color value
            byte color = 0;

            // Use a for loop instead of a foreach loop
            for (int i = 0; i < image.Length; i++)
            {
                color = image[i];
                coloredImage[i] = new Color(color, color, color, byte.MaxValue);
            }
            //Console.WriteLine($"img: {colorCount}");

            int paletteColorCount = input.ReadInt32(); //7: output.Write(value.Asset.palette.Length);

            Color[] decompressedPalettes = new Color[paletteColorCount];
            for (int i = 0; i < paletteColorCount; i++)
            {
                decompressedPalettes[i] = ColorHelper.CreateFromInteger((uint)input.ReadInt32()); //8: output.Write(paletteColor);
            }
            //Console.WriteLine($"pal: {paletteColorCount}");

            int spriteDefinitionCount = input.ReadInt32(); //9: output.Write(value.Asset.definitions.Count);
            string defaultSpriteDefinitionName = input.ReadString(); //10: output.Write(value.Asset.defaultDefinition.Name);
                                                                     // Console.WriteLine($"dname:{defaultSpriteDefinitionName}");

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

                List<float> speeds = new List<float>();
                int numberOfSpeeds = input.ReadInt32();
                // write animation speed
                for (int nOS = 0; nOS < numberOfSpeeds; nOS++)
                {
                    speeds.Add(input.ReadSingle());
                }

                //float speed = input.ReadSingle(); //19: output.Write(def.Speeds[0]);

                bool flipX = input.ReadBoolean(); //20: output.Write(def.FlipX);
                bool flipY = input.ReadBoolean(); //21: output.Write(def.FlipY);

                int mode = input.ReadInt32(); //22: output.Write((int)def.Mode);

                //hash the string!
                SpriteDefinition newDefinition = new SpriteDefinition(name, new Vector2(coordinatesX, coordinatesY), new Vector2(boundsX, boundsY), new Vector2(originX, originY), frames, speeds.ToArray(), flipX, flipY, mode, Array.Empty<int>());
                if (defaultSpriteDefinition == null && name == defaultSpriteDefinitionName)
                {
                    defaultSpriteDefinition = newDefinition;
                }
                spriteDefinitions.Add(name.GetHashCode(), newDefinition);

            }
            return new SpriteSheetTexture(imageWidth, imageHeight, paletteSize, totalPalettes, coloredImage, decompressedPalettes, spriteDefinitions, defaultSpriteDefinition);
            //return null;
        }
    }
}
