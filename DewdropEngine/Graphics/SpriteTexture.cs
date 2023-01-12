using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Dewdrop.Graphics
{
    public class SpriteTexture : IDisposable
    {
        #region Properties
        /// <summary>
        /// The texture of the image. 
        /// </summary>
        public Texture2D Texture
        {
            get => this.imageTex;
        }

        /// <summary>
        /// The texture of the palette. This is used by the GSS to apply color to the texture.
        /// </summary>
        public Texture2D Palette
        {
            get => this.paletteTex;
        }

        /// <summary>
        /// The current palette.
        /// </summary>
        public int CurrentPalette
        {
            get => this.currentPal;
            set
            {
                this.currentPal = Math.Min(this.totalPals, value);
            }
        }

        /// <summary>
        /// This float instance represents the current palette divided by the total amount of palettes.
        /// This is used by the GSS.
        /// </summary>
        public float CurrentPaletteFloat
        {
            get => (float)this.currentPal / (float)this.totalPals;
        }

        /// <summary>
        /// Represents how many palettes this instance contains.
        /// </summary>
        public int PaletteCount
        {
            get => this.totalPals;
        }

        /// <summary>
        /// Represents how many colors are in the current palette.
        /// </summary>
        public int PaletteSize
        {
            get => this.palSize;
        }

        #endregion

        private SpriteDefinition defaultDefinition;
        private Dictionary<int, SpriteDefinition> definitions;

        private Texture2D paletteTex;
        private Texture2D imageTex;

        private int currentPal;
        private int totalPals;
        private int palSize;
        private bool disposedValue;

        /// <summary>
        /// Creates a new IndexedTexture
        /// </summary>
        /// <param name="imageWidth">The width of the image.</param>
        /// <param name="imageHeight">The height of the image.</param>
        /// <param name="paletteSize">The size of the current palette</param>
        /// <param name="totalPalettes">Represents how many palettes this IndexedTexture contains.</param>
        /// <param name="image">The image, represented by an array of colors.</param>
        /// <param name="palette">The palette, represented by an array of colors.</param>
        /// <param name="definitions">The sprite definitions this instance has.</param>
        /// <param name="defaultDefinition">The default sprite definition this instance should use.</param>
        public SpriteTexture
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


        /// <summary>
        /// Returns a random sprite definition 
        /// </summary>
        /// <returns>A randomly chosen sprite definition</returns>
        public SpriteDefinition GetRandomSpriteDefinition()
        {
            return this.GetSpriteDefinition(definitions.ElementAt(new Random().Next(0, definitions.Count)).Key);

        }

        /// <summary>
        /// Gets a sprite definition by name.
        /// </summary>
        /// <param name="name">The name of the sprite definition you.</param>
        /// <returns>The sprite definition</returns>
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    definitions.Clear(); ;

                    // TODO: dispose managed state (managed objects)
                }

                imageTex.Dispose(); 
                paletteTex.Dispose();
                
                paletteTex = null;
                imageTex = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }           

        /// <summary>
        ///  override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        /// </summary>
        ~SpriteTexture()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
