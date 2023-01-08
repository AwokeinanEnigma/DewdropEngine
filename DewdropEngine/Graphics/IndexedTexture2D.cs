using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace DewdropEngine.Graphics
{
    public class IndexedTexture2D
    {

        #region Properties
        public Texture2D Image
        {
            get
            {
                return this.imageTex;
            }
        }

        public Texture2D Palette
        {
            get
            {
                return this.paletteTex;
            }
        }

        public uint CurrentPalette
        {
            get
            {
                return this.currentPal;
            }
            set
            {
                this.currentPal = Math.Min(this.totalPals, value);
            }
        }

        public float CurrentPaletteFloat
        {
            get
            {
                return (float)this.currentPal / (float)this.totalPals;
            }
        }

        public uint PaletteCount
        {
            get
            {
                return this.totalPals;
            }
        }

        public uint PaletteSize
        {
            get
            {
                return this.palSize;
            }
        }
        #endregion

        private SpriteDefinition defaultDefinition;

        private Dictionary<int, SpriteDefinition> definitions;

        private Texture2D paletteTex;
        private Texture2D imageTex;

        private uint currentPal;
        private uint totalPals;
        private uint palSize;

        private bool disposed;

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

        private Color[] totalColors;
        private Color[] uncoloredPixels;

        public unsafe IndexedTexture2D(uint width, int[][] palettes, byte[] image, Dictionary<int, SpriteDefinition> definitions, SpriteDefinition defaultDefinition)
        {
            this.totalPals = (uint)palettes.Length;
            this.palSize = (uint)palettes[0].Length; ;
            uint height = (uint)(image.Length / width);

           // Console.WriteLine(BitConverter.ToString(image));

            totalColors = new Color[this.palSize * this.totalPals];
            uncoloredPixels = new Color[width * height];

            for (int i = 0; i < totalColors.Length; i++)
            {
                totalColors[i] = FromInt(palettes[i / (int)this.palSize][i % (int)this.palSize]);
            }

            for (int i = 0; i < uncoloredPixels.Length; i++)
            {
                uncoloredPixels[i].A = byte.MaxValue;
                uncoloredPixels[i].R = image[i];
                uncoloredPixels[i].G = image[i];
                uncoloredPixels[i].B = image[i];
            }

            this.paletteTex = new Texture2D(Game1.instance.GraphicsDevice, (int)this.palSize, (int)this.totalPals);
            this.imageTex = new Texture2D(Game1.instance.GraphicsDevice, (int)width, (int)height);

            this.paletteTex.SetData(totalColors);
            this.imageTex.SetData(uncoloredPixels);
            this.definitions = definitions;
            this.defaultDefinition = defaultDefinition;
        }


        public SpriteDefinition GetRandomSpriteDefinition()
        {
            return this.GetSpriteDefinition(definitions.ElementAt(new Random().Next(0, definitions.Count)).Key);

        }

        public SpriteDefinition GetSpriteDefinition(string name)
        {

            int hashCode = name.GetHashCode();
            return this.GetSpriteDefinition(hashCode);
        }

        public SpriteDefinition GetSpriteDefinition(int hash)
        {
            if (!this.definitions.TryGetValue(hash, out SpriteDefinition result))
            {
                result = null;
            }
            return result;
        }

        public ICollection<SpriteDefinition> GetSpriteDefinitions()
        {
            return this.definitions.Values;
        }

        public SpriteDefinition GetDefaultSpriteDefinition()
        {
            return this.defaultDefinition;
        }

    }
}
