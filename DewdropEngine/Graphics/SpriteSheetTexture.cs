using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Dewdrop.Graphics.SpriteDefinition;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// Contains information about a spritesheet, such as its texture and palette.
    /// </summary>
    public class SpriteSheetTexture : IDisposable
    {
        #region Properties
        /// <summary>
        /// The texture of the spritesheet. 
        /// </summary>
        public Texture2D Texture
        {
            get => this._spritesheetTex;
        }

        /// <summary>
        /// The texture of the palette. This is used by the GSS to apply color to the texture.
        /// </summary>
        public Texture2D Palette
        {
            get => this._paletteTex;
        }

        /// <summary>
        /// The current palette.
        /// </summary>
        public int CurrentPalette
        {
            get => this._currentPal;
            set
            {
                this._currentPal = Math.Min(this._totalPals, value);
            }
        }

        /// <summary>
        /// This float instance represents the current palette divided by the total amount of palettes.
        /// This is used by the GSS.
        /// </summary>
        public float CurrentPaletteFloat
        {
            get => _currentPal / (float)this._totalPals;
        }

        /// <summary>
        /// Represents how many palettes this instance contains.
        /// </summary>
        public int PaletteCount
        {
            get => this._totalPals;
        }

        /// <summary>
        /// Represents how many colors are in the current palette.
        /// </summary>
        public int PaletteSize
        {
            get => this._palSize;
        }

        #endregion

        private readonly SpriteDefinition _defaultDefinition;
        private readonly Dictionary<int, SpriteDefinition> _definitions;

        private Texture2D _paletteTex;
        private Texture2D _spritesheetTex;

        private int _currentPal;
        private readonly int _totalPals;
        private readonly int _palSize;
        private bool _disposedValue;

        /// <summary>
        /// Creates a new SpriteSheetTexture
        /// </summary>
        /// <param name="spritesheetWidth">The width of the spritesheet.</param>
        /// <param name="spritesheetHeight">The height of the spritesheet.</param>
        /// <param name="paletteSize">The size of the current palette</param>
        /// <param name="totalPalettes">Represents how many palettes this IndexedTexture contains.</param>
        /// <param name="spritesheet">The spritesheet, represented by an array of colors.</param>
        /// <param name="palette">The palette, represented by an array of colors.</param>
        /// <param name="definitions">The sprite definitions this instance has.</param>
        /// <param name="defaultDefinition">The default sprite definition this instance should use.</param>
        public SpriteSheetTexture
            (
            int spritesheetWidth,
            int spritesheetHeight,
            int paletteSize,
            int totalPalettes,
            Color[] spritesheet,
            Color[] palette,
            Dictionary<int, SpriteDefinition> definitions,
            SpriteDefinition defaultDefinition
            )
        {


            this._palSize = paletteSize;
            this._totalPals = totalPalettes;

            this._paletteTex = new Texture2D(Engine.instance.GraphicsDevice, _palSize, _totalPals);
            this._spritesheetTex = new Texture2D(Engine.instance.GraphicsDevice, spritesheetWidth, spritesheetHeight);

            this._paletteTex.SetData(palette);
            this._spritesheetTex.SetData(spritesheet);

            this._definitions = definitions;
            this._defaultDefinition = defaultDefinition;
        }


        /// <summary>
        /// Returns a random sprite definition 
        /// </summary>
        /// <returns>A randomly chosen sprite definition</returns>
        public SpriteDefinition GetRandomSpriteDefinition()
        {
            return this.GetSpriteDefinition(_definitions.ElementAt(new Random().Next(0, _definitions.Count)).Key);

        }

        /// <summary>
        /// Gets a sprite definition by name.
        /// </summary>
        /// <param name="name">The name of the sprite definition you.</param>
        /// <returns>The sprite definition</returns>
        public SpriteDefinition GetSpriteDefinition(string name)
        {

            int hashCode = name.GetHashCode();
            SpriteDefinition def = GetSpriteDefinition(hashCode);
            if (def == null)
            {
                throw new SpriteDefinitionNotFoundException(name);
            }
            return def;
        }

        /// <summary>
        /// Gets a sprite definition by hash.
        /// </summary>
        /// <param name="hash">The integer hash of the name of the sprite definition.</param>
        /// <returns>The sprite definition</returns>
        public SpriteDefinition GetSpriteDefinition(int hash)
        {
            if (!this._definitions.TryGetValue(hash, out SpriteDefinition result))
            {
                result = null;
            }
            return result;
        }

        public ICollection<SpriteDefinition> GetSpriteDefinitions()
        {
            return this._definitions.Values;
        }

        /// <summary>
        /// Returns the default sprite definition of the sprite sheet
        /// </summary>
        /// <returns>The default definition of the spritesheet</returns>
        public SpriteDefinition GetDefaultSpriteDefinition()
        {
            return this._defaultDefinition;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _definitions.Clear(); ;

                    // TODO: dispose managed state (managed objects)
                }

                _spritesheetTex.Dispose();
                _paletteTex.Dispose();

                _paletteTex = null;
                _spritesheetTex = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        /// <summary>
        ///  override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        /// </summary>
        ~SpriteSheetTexture()
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
