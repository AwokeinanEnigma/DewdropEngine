using Microsoft.Xna.Framework.Content.Pipeline;
using System;

namespace Dewdrop.PipelineExtensions.GDat
{
    [ContentProcessor(DisplayName = "Graphic DAT Processor - Dewdrop")]
    internal class GDatProcessor : ContentProcessor<GDatContent, GDatContent>
    {
        public override GDatContent Process(GDatContent input, ContentProcessorContext context)
        {
            //ValidateTexture(input); 
            return input;
        }

        public void ValidateTexture(GDatContent texture)
        {
            if (texture.Asset.palette.Length > 256)
            {
                throw new Exception("Texture has more than 256 colors!!");
            }

        }
    }
}
