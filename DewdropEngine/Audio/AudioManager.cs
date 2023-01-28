using Dewdrop.Audio.Raw_FMOD;
using Dewdrop.Debugging;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Audio
{
    public class AudioManager
    {
        private Raw_FMOD.System _system;
        private struct FMODInfo {
            public int numberOfDrivers;
            public int fmodVersion;
        }

        public AudioManager() {

            // first check if FMOD can actually be used
            RESULT sysCheck = Factory.System_Create(out _system);
            if (sysCheck == RESULT.OK)
            {
                // this is going to be passed around so instead of making three RESULT variables i can just make one and reuse it
                RESULT misc =  RESULT.OK;

                // for some reason this wants System.Guid instead of FMOD's own GUID 
                // it is what it is
                Guid driverInfo = new Guid();
                _system.getDriverInfo(0, out string driverName, 256, out driverInfo, out int systemRate, out SPEAKERMODE speakerMode, out int channelNumber);

                DBG.LogUnique("FMOD Audio",
                    ConsoleColor.Cyan,
                    $"Current driver info:"
                    + Environment.NewLine +
                    $"Driver Name: {driverName}"
                    + Environment.NewLine +
                    $"GUID Info: {driverInfo}"
                    + Environment.NewLine +
                    $"Current System Rate (if lower than 48 there is a problem): {systemRate}"
                    + Environment.NewLine +
                    $"Speaker Mode: {speakerMode}"
                    + Environment.NewLine +
                    $"Number of channels: {channelNumber}");




            }
            else {
                throw new Exception($"FMOD couldn't be loaded! "
                    + Environment.NewLine +
                    $"FMOD ERROR: {sysCheck}"
                    + Environment.NewLine +
                    $"FMOD ERROR TO STRING: {Error.String(sysCheck)}");
            }
        }

    }
}
