using Dewdrop.Utilities.fNbt;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Dewdrop.PipelineExtensions.NBT
{
    [ContentTypeWriter]
    public sealed class NBTWriter : ContentTypeWriter<NBTContent>
    {
        /// <inheritdoc />
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
            => typeof(Dewdrop.PipelineReaders.IndexedTexture2DReader).AssemblyQualifiedName ?? string.Empty;

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Dewdrop.PipelineReaders.IndexedTexture2DReader).AssemblyQualifiedName ?? string.Empty;
        }

        /// <inheritdoc />
        protected override void Write(ContentWriter output, NBTContent value)
        {
            byte[] buffer = value.Asset.SaveToBuffer(NbtCompression.GZip);
            output.Write(buffer.Length);
            output.Write((int)NbtCompression.GZip);
            output.Write(buffer);

        }
    }

}
