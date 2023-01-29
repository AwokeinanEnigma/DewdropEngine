using Dewdrop.PipelineExtensions.Base;
using Dewdrop.Utilities.fNbt;

namespace Dewdrop.PipelineExtensions.NBT
{
    public sealed class NBTContent : ContentItem<NbtFile>
    {
        /// <summary>
            /// Initializes a new instance of the <see cref="IndexedTexture2DTest"/> class.
            /// </summary>
            /// <param name="asset">The configuration data for the sprite sheet.</param>
        public NBTContent(NbtFile asset)
    : base(asset)
        { }
    }
}