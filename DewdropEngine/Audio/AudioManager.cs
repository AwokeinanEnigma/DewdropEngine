using Dewdrop.Audio.Raw_FMOD;
using Dewdrop.Debugging;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Dewdrop.Audio
{
    public class AudioManager
    {
        /// <summary>
        /// The low level FMOD sound system.
        /// </summary>
        public Raw_FMOD.System System {
            get 
            {
                return _system;
            }
                
        }

        private Raw_FMOD.System _system;
        private FMODInfo _info;
        private struct FMODInfo
        {
            public int numberOfDrivers;
            public uint fmodVersion;

            public FMODInfo(Raw_FMOD.System sys) {
                sys.getNumDrivers(out numberOfDrivers);
                sys.getVersion(out fmodVersion);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static string ConvertResultToString(RESULT result)
        {
            return $"Result: {result} - FMOD Description: {Error.String(result)}";
        }

        public AudioManager() {

            // first check if FMOD can actually be used
            RESULT sysCheck = Factory.System_Create(out _system);
            if (sysCheck == RESULT.OK)
            {
                // this is going to be passed around so instead of making three RESULT variables i can just make one and reuse it
                RESULT misc =  RESULT.OK;

                _info = new FMODInfo(_system);

                // for some reason this wants System.Guid instead of FMOD's own GUID 
                // it is what it is
                Guid driverInfo = new Guid();
                misc = _system.getDriverInfo(0, out string driverName, 256, out driverInfo, out int systemRate, out SPEAKERMODE speakerMode, out int channelNumber);
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
                
                if (misc != RESULT.OK) {
                    DBG.LogError($"Error while trying to get driver info. {ConvertResultToString(misc)}", null);
                }

                if (_info.numberOfDrivers == 0)
                {
                    // now, theroetically, this shouldn't be possible
                    // because ideally in 2023, every computer should have an audio driver
                    // buuuut just in case!
                    _system.setOutput(OUTPUTTYPE.NOSOUND);
                    DBG.LogError($"The user's computer does not have an audio driver!", null);
                }

                InitiateFMODSystem();

            }
            else {
                throw new Exception($"FMOD couldn't be loaded! "
                                    + Environment.NewLine +
                                    $"FMOD ERROR: {sysCheck}"
                                    + Environment.NewLine +
                                    $"FMOD ERROR TO STRING: {Error.String(sysCheck)}");
            }
        }

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
            }
            return bufferRes;
        }

        /// <summary>
        /// Loads streamed sound stream from file.
        /// Use this function to load music and long ambience tracks.
        /// </summary>
        public StreamedSound LoadStreamedSound(string path)
        {
            var buffer = LoadFileAsBuffer(path);

            // Internal FMOD pointer points to this memory, so we don't want it to go anywhere.
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            var info = new CREATESOUNDEXINFO();
            info.length = (uint)buffer.Length;
            info.cbsize = Marshal.SizeOf(info);

            _system.createStream(
                    buffer,
                    MODE.OPENMEMORY | MODE.CREATESTREAM,
                    ref info,
                    out Sound newSound
            );

            return new StreamedSound(newSound, buffer, handle);
        }

        /// <summary>
        /// Loads sound from file.
        /// Use this function to load short sound effects.
        /// </summary>
        public SampleSound LoadSampleSound(string path)
        {
            var buffer = LoadFileAsBuffer(path);

            var info = new CREATESOUNDEXINFO();
            info.length = (uint)buffer.Length;
            info.cbsize = Marshal.SizeOf(info);

            _system.createSound(
                buffer,
                MODE.OPENMEMORY | MODE.CREATESAMPLE,
                ref info,
                out Sound newSound
            );

            return new SampleSound(newSound);
        }

        public unsafe void InitiateFMODSystem()
        {
            RESULT res = _system.init(64, INITFLAGS.NORMAL, (IntPtr)(void*)null);
            if (res != RESULT.OK)
            {
                throw new Exception($"FMOD system couldn't be initiated! "
                                    + Environment.NewLine +
                                    $"FMOD ERROR: {res}"
                                    + Environment.NewLine +
                                    $"FMOD ERROR TO STRING: {Error.String(res)}");
            }
        }
    }
}

