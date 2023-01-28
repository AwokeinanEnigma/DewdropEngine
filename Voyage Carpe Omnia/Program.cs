using Dewdrop.Audio.Raw_FMOD;
using Dewdrop.Debugging;
using Microsoft.Xna.Framework.Audio;
using System;

namespace VCO
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            object a;
            a = SoundState.Playing;

            DBG.Log(a);

            try
            {
                using (var game = new VoyageCarpeOmnia())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                // avoiding the debug class because if an error occurred there then everything's screwed
                DBG.LogError($"Encountered error: {ex}", null);
                DBG.DumpLogs();
            }
        }
    }
}