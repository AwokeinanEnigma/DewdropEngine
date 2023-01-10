using Dewdrop.PipelineExtensions.Base;
using Dewdrop.Utilities.fNbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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