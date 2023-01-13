using Dewdrop.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.IO;
using System.IO.Compression;

namespace Dewdrop.PipelineExtensions.GDat
{
    /// <summary>
    /// Provides a writer of raw sprite sheet content to the content pipeline.
    /// </summary>
    [ContentTypeWriter]
    public sealed class GDatWriter : ContentTypeWriter<GDatContent>
    {
        /// <inheritdoc />
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
            => typeof(Dewdrop.PipelineReaders.IndexedTexture2DReader).AssemblyQualifiedName ?? string.Empty;

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(PipelineReaders.IndexedTexture2DReader).AssemblyQualifiedName ?? string.Empty;
        }


        /// <inheritdoc />
        protected override void Write(ContentWriter output, GDatContent value)
        {
            output.Write(value.Asset.width);
            output.Write(value.Asset.height);

            output.Write(value.Asset.totalPals);
            output.Write(value.Asset.palSize);

            output.Write(value.Asset.img.Length);

            // compress palette into single int! ---------
            output.Write(value.Asset.img);

            output.Write(value.Asset.palette.Length);
            foreach (Color paletteColor in value.Asset.palette)
            {
                output.Write(ColorHelper.ToInt(paletteColor));

            }

            // okay clusterfuck time!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // write sprite definitions ---------


            output.Write(value.Asset.definitions.Count);
            output.Write(value.Asset.defaultDefinition.Name);

            foreach (PipelineSpriteDefinition def in value.Asset.definitions)
            {

                output.Write(def.Name);

                // write coords: x then y
                output.Write(def.Coords.X);
                output.Write(def.Coords.Y);

                // write bounds: x then y
                output.Write(def.Bounds.X);
                output.Write(def.Bounds.Y);

                // write origin: x then y
                output.Write(def.Origin.X);
                output.Write(def.Origin.Y);

                // write frames
                output.Write(def.Frames);

                output.Write(def.Speeds.Length);
                // write animation speed
                for (int i = 0; i < def.Speeds.Length; i++)
                {
                    output.Write(def.Speeds[i]);
                }

                // write flip bools
                output.Write(def.FlipX);
                output.Write(def.FlipY);

                // write animationmode
                output.Write((int)def.Mode);

                // write data
                // TODO: find way to pack data into a single int instead of doing a for loop
                // output.Write(def.Data[0]);
            }
        }
    }

}