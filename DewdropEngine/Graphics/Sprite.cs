using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Dewdrop.Graphics
{
    public class Sprite : Renderable
    {
        /// <summary>
        /// The current palette to use. Starts at 0.
        /// </summary>
        public int Palette {
            get => _paletteIndex;
            set {
                _paletteIndex = value; 

            }
        }

        /// <summary>
        /// Should this sprite be flipped horizontally?
        /// </summary>
        public bool FlipX
        {
            get => _flipX;
            set => _flipX = value;
        }

        /// <summary>
        /// Should this sprite be flipped vertically
        /// </summary>
        public bool FlipY
        {
            get => _flipY;
            set => _flipY = value;
        }

        private Rectangle _spriteRect;
        private SpriteTexture _texture;
        private Effect _shader;

        private int _paletteIndex;

        private SpriteDefinition _currentDefinition;

        private bool _flipX;
        private bool _flipY;

        public Sprite(SpriteTexture texture, Effect shader, string defaultSpriteName, int depth, int palette, Vector2 position, string name = "")
        {
            _texture = texture;
            _shader = shader;
            _depth = depth;
            _position = position;
            _visible = true;
            _paletteIndex = palette;
            
            base.name = name;
            
            SwitchSprite(defaultSpriteName);
        }

        public void SwitchSprite(string spriteName) {
            SpriteDefinition newDef = _texture.GetSpriteDefinition(spriteName.GetHashCode());
            if (newDef == this._currentDefinition) {
                Logger.LogWarning("Tried to set an IndexedColorGraphic's sprite definition to the same definition.");
                return;
            }

            _currentDefinition = newDef;

            // create new rectangle
            _spriteRect = new Rectangle((int)_currentDefinition.Coords.X, (int)_currentDefinition.Coords.Y, (int)_currentDefinition.Bounds.X, (int)_currentDefinition.Bounds.Y);
        
            _flipX = _currentDefinition.FlipX;
            _flipY = _currentDefinition.FlipY;
        }

        public void Save() {
            Stream stream = File.Create("tex.png");
            Stream stream2 = File.Create("pal.png");
            _texture.Texture.SaveAsPng(stream, _texture.Texture.Width, _texture.Texture.Height);
            _texture.Palette.SaveAsPng(stream2, _texture.Palette.Width, _texture.Palette.Height);
            stream.Dispose();
            stream2.Dispose();
        }

        public override void Draw(SpriteBatch batch)
        {
            _texture.CurrentPalette = _paletteIndex;
            _shader.Parameters["img"].SetValue(_texture.Texture);
            _shader.Parameters["pal"].SetValue(_texture.Palette);

            _shader.Parameters["palIndex"].SetValue(_texture.CurrentPaletteFloat);
            _shader.Parameters["palSize"].SetValue(_texture.PaletteSize);
            _shader.Parameters["blend"].SetValue(Color.White.ToVector4());
            _shader.Parameters["blendMode"].SetValue(1);
            _shader.CurrentTechnique.Passes[0].Apply();

            batch.Begin(effect: _shader);

            batch.Draw(
                texture: _texture.Texture,
                position: _position,
                sourceRectangle: _spriteRect,
                color: Color.White
                );

            // draw code here

            batch.End();
        }
    }
}
