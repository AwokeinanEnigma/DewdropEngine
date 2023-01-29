using Dewdrop.Utilities.fNbt;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Dewdrop.PipelineExtensions.NBT
{
    [ContentImporter(".nbt", DisplayName = "NBT File Importer - Dewdrop", DefaultProcessor = nameof(NBTProcessor))]
    public class NBTImporter : ContentImporter<NBTContent>
    {
        public override NBTContent Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage($"Loading .nbt file: {filename}");

            // pass image to method 
            return new NBTContent(new NbtFile(filename));
        }
    }
}
