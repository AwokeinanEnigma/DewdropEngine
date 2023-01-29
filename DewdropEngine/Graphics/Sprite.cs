using Dewdrop.Debugging;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Dewdrop.Audio.Raw_FMOD;
using static Dewdrop.Graphics.SpriteDefinition;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// A sprite is an animated renderable that reads a texture, palette, and sprite definitions from an SpriteTexture and uses a shader to apply a palette to the texture.
    /// </summary>
    public class Sprite : AnimatedRenderable
    {
        /// <summary>
        /// The current palette to use. Starts at 0.
        /// </summary>
        public int Palette
        {
            get => _paletteIndex;
            set
            {
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

        /// <summary>
        /// Has this sprite been disposed?
        /// </summary>
        public bool Disposed
        {
            get => hasDisposed;
        }


        public override Rectangle RenderableRectangle
        {
            get => new Rectangle((int)_position.X, (int)_position.Y, _spriteRect.Width, _spriteRect.Height);
        }

        private Rectangle _spriteRect;
        private Rectangle _texReact;
        private SpriteSheetTexture _texture;
        private readonly Effect _shader;

        private int _paletteIndex;

        private SpriteDefinition _currentDefinition;

        private bool _flipX;
        private bool _flipY;

        private SpriteAnimationMode _animationMMode;

        private float _betaFrame;

        private static readonly int[] MODE_ONE_FRAMES = new int[] {
            0,
            1,
            0,
            2
        };


        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="texture">The sprite texture to pull the texture and palette from.</param>
        /// <param name="shader">The shader to use.</param>
        /// <param name="defaultSpriteName">The default sprite to use.</param>
        /// <param name="depth">The depth of the sprite, used by the render pipeline.</param>
        /// <param name="palette">The palette index to use.</param>
        /// <param name="position">The location of the sprite on screen.</param>
        /// <param name="name">The name of the sprite, used for debugging purposes</param>
        public Sprite(SpriteSheetTexture texture, Effect shader, string defaultSpriteName, int depth, int palette, Vector2 position, string name = "")
        {
            _texture = texture;
            _shader = shader;
            _depth = depth;
            _position = position;
            _visible = true;
            _paletteIndex = palette;

            _color = Color.White;

            base.name = name;

            //Engine.RenderDebugUI += Engine_RenderDebugUI;

            SetSpriteDef(defaultSpriteName);
        }

        private void Engine_RenderDebugUI(DewGui.ImGuiRenderer renderer)
        {
            ImGui.BeginGroup();
            ImGui.BeginPopup("schmovement");
            if (ImGui.Button("push up"))
            {
                _position += new Vector2(0, -3);
            }
            if (ImGui.Button("push down"))
            {
                _position += new Vector2(0, 3);
            }
            if (ImGui.Button("push left"))
            {
                _position += new Vector2(-3, 0);
            }
            if (ImGui.Button("push right"))
            {
                _position += new Vector2(3, 0);
            }
            ImGui.EndPopup();
            ImGui.EndGroup();
        }

        /// <summary>
        /// Sets the sprite definition of the sprtite.
        /// </summary>
        /// <param name="spriteName">The name of the sprite definition</param>
        /// <param name="resetAnimation">If true, this will ensure that the animation starts at the very start</param>
        /// <exception cref="DisposedObjectException">If this method is called when this sprite is disposed, this exception will be thrown</exception>
        public void SetSpriteDef(string spriteName, bool resetAnimation = true)
        {
            if (hasDisposed)
            {
                // prevent people from using disposed sprites

                throw new DisposedObjectException(name);
            }

            SpriteDefinition newDef = _texture.GetSpriteDefinition(spriteName);
            if (newDef == this._currentDefinition)
            {
                DBG.LogWarning("Tried to set an sprite's sprite definition to the same definition.");
                return;
            }


            _currentDefinition = newDef;

            // create new rectangle
            _spriteRect = new Rectangle((int)_currentDefinition.Coords.X, (int)_currentDefinition.Coords.Y, (int)_currentDefinition.Bounds.X, (int)_currentDefinition.Bounds.Y);
            _size = new Vector2(_spriteRect.Width, _spriteRect.Height);
            _texReact = _texture.Texture.Bounds;


            _flipX = _currentDefinition.FlipX;
            _flipY = _currentDefinition.FlipY;

            _totalFrames = newDef.Frames;
            _speeds = newDef.Speeds;
            _animationMMode = newDef.Mode;

            if (resetAnimation)
            {
                _currentFrame = 0f;
                _betaFrame = 0f;
                _speedIndex = 0f;
                _speedModifier = 1f;
                return;
            }
            this._currentFrame %= _totalFrames;
        }

        /// <summary>
        /// Saves the texture and palette of this sprite to .png files.
        /// </summary>
        public void Save()
        {
            Stream stream = File.Create("tex.png");
            Stream stream2 = File.Create("pal.png");
            _texture.Texture.SaveAsPng(stream, _texture.Texture.Width, _texture.Texture.Height);
            _texture.Palette.SaveAsPng(stream2, _texture.Palette.Width, _texture.Palette.Height);
            stream.Dispose();
            stream2.Dispose();
        }

        protected void UpdateAnimation()
        {
            //store the result of the multiplication in a variable to avoid recalculating it
            int currentFrameIndex = (int)this._currentFrame * (int)this._size.X;
            //calculate left position by taking the modulo of currentFrameIndex and the width of the texture
            int frameLeftPosition = currentFrameIndex % _texture.Texture.Width;
            //calculate top position by dividing currentFrameIndex by the width of the texture and multiplying by the height of each frame
            int frameTopPosition = currentFrameIndex / _texture.Texture.Width * (int)this._size.Y;

            //create the rectangle for the sprite using the calculated frameLeftPosition and frameTopPosition and the width and height of each frame
            this._spriteRect = new Rectangle(frameLeftPosition, frameTopPosition, (int)this._size.X, (int)this._size.Y);

            //update the frame and check if the animation has completed
            this._speedIndex = (this._speedIndex + this.GetFrameSpeed()) % _speeds.Length;
            if (this._currentFrame + this.GetFrameSpeed() >= _totalFrames)
            {
                OnAnimationCompleted();
            }
            this.IncrementFrame();
        }

        private float _frameTimer;

        protected virtual void IncrementFrame()
        {
            float frameSpeed = GetFrameSpeed();

            // going to bed
            // buuuuut!
            // before i forget
            // i think i'm misunderstanding the system
            // if i change this._currentFrame = (this._currentFrame + frameSpeed) % _currentFrame ;
            // i'm fucking with the wrong code here, i need to change UpdateAnimation.

            _frameTimer += Engine.DeltaTime * frameSpeed;
            if (_frameTimer >= frameSpeed)
            {
                _frameTimer = 0;
                switch (_animationMMode)
                {
                    case SpriteAnimationMode.Continous:
                        this._currentFrame = _currentFrame + 1;
                        break;

                    case SpriteAnimationMode.ZeroTwoOneThree:
                        this._betaFrame = (this._betaFrame) % 4f;
                        this._currentFrame = MODE_ONE_FRAMES[(int)this._betaFrame];
                        break;
                }
            }
            _speedIndex = (int)this._currentFrame % this._speeds.Length;
        }

        protected float GetFrameSpeed()
        {
            return _speeds[(int)Math.Min(_speedIndex, _speeds.Length - 1)] * _speedModifier;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!hasDisposed)
            {
                if (_totalFrames > 1 && _animationEnabled)
                {
                    UpdateAnimation();
                }

                _texture.CurrentPalette = _paletteIndex;
                _shader.Parameters["img"].SetValue(_texture.Texture);
                _shader.Parameters["pal"].SetValue(_texture.Palette);

                _shader.Parameters["palIndex"].SetValue(_texture.CurrentPaletteFloat);
                _shader.Parameters["palSize"].SetValue(_texture.PaletteSize);
                _shader.Parameters["blend"].SetValue(_color.ToVector4());
                _shader.Parameters["blendMode"].SetValue(1);


                _shader.CurrentTechnique.Passes[0].Apply();

                //todo: optimize this ugly thing
                SpriteEffects effect = SpriteEffects.None;
                if (_flipX)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                if (_flipY)
                {
                    // help
                    effect = effect | SpriteEffects.FlipVertically;
                }


                //batch.Begin(effect: _shader);

                batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, _shader, Camera.Instance.Matrix * Engine.ScreenMatrix);

                batch.Draw(_texture.Texture, _position, _spriteRect, Color.White, 0, Vector2.Zero, Vector2.One, effect, 0.0f);

                /*batch.Draw(
                    texture: _texture.Texture,
                    position: _position,
                    sourceRectangle: _spriteRect,
                    color: Microsoft.Xna.Framework.Color.White
                    );*/


                // draw code here

                batch.End();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (disposing)
                {
                    _currentDefinition = null;
                    _speeds = null;
                    _texture = null;

                    // TODO: dispose managed state (managed objects)
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                hasDisposed = true;
            }
            else
            {
                throw new DisposedObjectException(this.name);
            }
        }

        ~Sprite()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
    }
}
