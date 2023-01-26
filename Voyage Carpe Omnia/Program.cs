using Dewdrop.Debugging;
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
                DBG.LogError($"Error occurred either within game or engine: {ex}", null);
                DBG.DumpLogs();
            }
        }
    }
}