using Microsoft.Xna.Framework;
using System;

namespace Dewdrop.Graphics
{
    /// <summary>
    /// A sprite definition contains information about a part of a spritesheet. 
    /// </summary>
    public class SpriteDefinition
    {
        public class SpriteDefinitionNotFoundException : Exception
        {
            public SpriteDefinitionNotFoundException(string name) : base($"Tried to get a sprite definition that doesn't exist! Sprite Definition: {name}") { }
        }

        /// <summary>
        /// Contains options for different modes of animation.
        /// </summary>
        public enum SpriteAnimationMode
        {

            Invalid = -1,

            /// <summary>
            /// Makes the animation go in order. Ex. 0 -> 1 -> 2 
            /// </summary>
            Continous = 0,

            /// <summary>
            /// Makes the animation go 0 -> 2 -> 1 -> 3.
            /// </summary>
            ZeroTwoOneThree = 1,

            /// <summary>
            /// The max amount of options in this enum.
            /// </summary>
            Maximum
        }

        /// <summary>
        /// The name of the sprite
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The coords of the sprite
        /// </summary>
        public Vector2 Coords { get; private set; }

        /// <summary>
        /// The bounds of the sprite
        /// </summary>
        public Vector2 Bounds { get; private set; }

        /// <summary>
        /// The origin of the sprite
        /// </summary>
        public Vector2 Origin { get; private set; }

        /// <summary>
        /// How many frames this sprite definition has
        /// </summary>
        public int Frames { get; private set; }

        /// <summary>
        /// The speeds of the animations within the sprite definitions
        /// </summary>
        public float[] Speeds { get; private set; }

        /// <summary>
        /// Should we flip this sprite horizontally?
        /// </summary>
        public bool FlipX { get; private set; }

        /// <summary>
        /// Should we flip this sprite vertically
        /// </summary>
        public bool FlipY { get; private set; }

        /// <summary>
        /// What mode this sprite definition is in
        /// </summary>
        public SpriteAnimationMode Mode { get; private set; }

        /// <summary>
        /// Additional data
        /// </summary>
        public int[] Data { get; private set; }


        /// <summary>
        /// Creates a new sprite definition
        /// </summary>
        /// <param name="name">The name of the Sprite Definition</param>
        /// <param name="coords">The coordinates of the sprite definition relat</param>
        /// <param name="bounds"></param>
        /// <param name="origin"></param>
        /// <param name="frames"></param>
        /// <param name="speeds"></param>
        /// <param name="flipX"></param>
        /// <param name="flipY"></param>
        /// <param name="mode"></param>
        /// <param name="data">Extra data</param>
        public SpriteDefinition(string name, Vector2 coords, Vector2 bounds, Vector2 origin, int frames, float[] speeds, bool flipX, bool flipY, int mode, int[] data)
        {
            this.Name = name;
            this.Coords = coords;
            this.Bounds = bounds;
            this.Origin = origin;
            this.Frames = frames;
            this.Speeds = speeds;
            this.FlipX = flipX;
            this.FlipY = flipY;
            this.Mode = (SpriteAnimationMode)mode;
            this.Data = data;
        }

        /// <summary>
        /// Creates a new sprite definition by copying a pre-existing sprite definition.
        /// </summary>
        public SpriteDefinition(SpriteDefinition spr)
        {
            this.Name = spr.Name;
            this.Coords = spr.Coords;
            this.Bounds = spr.Bounds;
            this.Origin = spr.Origin;
            this.Frames = spr.Frames;
            this.Speeds = spr.Speeds;
            this.FlipX = spr.FlipX;
            this.FlipY = spr.FlipY;
            this.Mode = spr.Mode;
            this.Data = spr.Data;
        }
    }
}
