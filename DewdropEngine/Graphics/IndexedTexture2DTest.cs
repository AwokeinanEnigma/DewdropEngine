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
    public class IndexedTexture2DTest
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

        public int CurrentPalette
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

        public int PaletteCount
        {
            get
            {
                return this.totalPals;
            }
        }

        public int PaletteSize
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

        private int currentPal;
        private int totalPals;
        private int palSize;

        public IndexedTexture2DTest
            (
            int imageWidth,
            int imageHeight,
            int paletteSize,
            int totalPalettes,
            Color[] image,
            Color[] palette,
            Dictionary<int, SpriteDefinition> definitions, 
            SpriteDefinition defaultDefinition
            )
        {

            this.palSize = paletteSize;
            this.totalPals = totalPalettes;

            this.paletteTex = new Texture2D(Game1.instance.GraphicsDevice, (int)this.palSize, (int)this.totalPals);
            this.imageTex = new Texture2D(Game1.instance.GraphicsDevice, (int)imageWidth, (int)imageHeight);

            this.paletteTex.SetData(palette);
            this.imageTex.SetData(image);

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
