using Microsoft.Xna.Framework;

namespace DewdropEngine.Utilities
{
    /// <summary>
    /// Provides cross-platform methods to load a files.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Loads the file from the specified path as a buffer.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <returns>The contents of the file as a byte array.</returns>
        public static byte[] LoadFileAsBuffer(string path)
        {
            // NOTE: Use this method to load audio files from memory 
            // instead of built-in methods which load files directly.
            // They will not work on some platforms.

            // TitleContainer is cross-platform Monogame file loader.
            var stream = TitleContainer.OpenStream(Path.Combine("Content", path));

            // File is opened as a stream, so we need to read it to the end.
            var buffer = new byte[16 * 1024];
            byte[] bufferRes;
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                bufferRes = ms.ToArray();
                ms.Close();
                stream.Close();
            }
            return bufferRes;
        }
    }
}
