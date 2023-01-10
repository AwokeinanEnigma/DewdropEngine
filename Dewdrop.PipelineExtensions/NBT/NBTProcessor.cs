using Dewdrop.PipelineExtensions.GDat;
using Dewdrop.Utilities.fNbt;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.PipelineExtensions.NBT
{
    [ContentProcessor(DisplayName = "NBT Processor - Dewdrop")]
    internal class NBTProcessor : ContentProcessor<NBTContent, NBTContent>
    {
        public override NBTContent Process(NBTContent input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
