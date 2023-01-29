using Microsoft.Xna.Framework.Content.Pipeline;

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
