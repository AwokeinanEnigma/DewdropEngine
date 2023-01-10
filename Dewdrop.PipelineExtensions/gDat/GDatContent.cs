using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dewdrop.PipelineExtensions.Base;

namespace Dewdrop.PipelineExtensions.GDat
{
    public sealed class GDatContent : ContentItem<PipelineIndexedTexture>
    {
        /// <summary>
            /// Initializes a new instance of the <see cref="PipelineIndexedTexture"/> class.
            /// </summary>
            /// <param name="asset">The configuration data for the sprite sheet.</param>
        public GDatContent(PipelineIndexedTexture asset)
    : base(asset)
        { }
    }
}
