using Dewdrop.Audio.Raw_FMOD;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dewdrop.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamedSound : IDisposable
    {
        public readonly Sound NativeSound;
        private Channel _nativeChannel;

        private bool _hasDisposed;

        #region Properties
        /// <summary>
        /// Returns whether or not the sound is paused.
        /// </summary>
        public bool Paused { 
            get 
            {
                _nativeChannel.getPaused(out bool paused);
                return paused;
            }
            set => _nativeChannel.setPaused(value);
             
        }

        /// <summary>
        /// Returns whether or not the sound is looping.
        /// </summary>
        public bool Looping
        {
            get
            {
                _nativeChannel.getLoopCount(out int loops);
                return (loops == -1);
            }
            set
            {
                if (value)
                {
                    Loops = -1;
                }
                else
                {
                    Loops = 0;
                }
            }
        }

        /// <summary>
        /// Sound length milliseconds
        /// </summary>
        public uint Length
        {
            get
            {
                NativeSound.getLength(out var length, TIMEUNIT.MS);
                return length;
            }
        }

        /// <summary>
        /// Amount of loops. 
        /// > 0 - Specific count.
        /// 0 - No loops.
        /// -1 - Infinite loops.
        /// </summary>
        public int Loops
        {
            get
            {
                // Do you have some lööps, bröther?
                // NO, I DON'T 
                // FUCK YOU
                _nativeChannel.getLoopCount(out var loops);
                return loops;
            }
            set
            {
                if (value == 0)
                {
                    Mode = MODE.LOOP_OFF;
                }
                else
                {
                    Mode = MODE.LOOP_NORMAL;
                }

                _nativeChannel.setLoopCount(value);
            }
        }

        /// <summary>
        /// Sound mode.
        /// </summary>
        public MODE Mode
        {
            get
            {
                _nativeChannel.getMode(out MODE mode);
                return mode;
            }
            set => _nativeChannel.setMode(value);
        }

        /// <summary>
        /// How far the song has progressed in milliseconds.
        /// </summary>
        public uint Position
        {
            get
            {

                _nativeChannel.getPosition(out uint position, TIMEUNIT.MS);
                return position;
            }
            set
            {
                _nativeChannel.setPosition(value, TIMEUNIT.MS);
            }
        }
        #endregion


        /// <summary>
        /// Sound buffer. Used for streamed sounds, which point to this memory.
        /// In other words, we need to just reference it somewhere to prevent
        /// garbage collector from collecting it.
        /// This memory is also pinned, so GC won't move it anywhere.
        /// 
        /// If any unexpected crashes emerge, this is the first suspect.
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// Buffer's handle.
        /// </summary>
        private GCHandle _bufferHandle;


        internal StreamedSound(Raw_FMOD.Sound sound, byte[] buffer, GCHandle bufferHandle)
        {
            NativeSound = sound;
            _buffer = buffer;
            _bufferHandle = bufferHandle;
        }

        #region Playing methods
        /// <summary>
        /// Plays the sound in a group.
        /// </summary>
        /// <param name="group">The group to play the sound in</param>
        public void Play(ChannelGroup group)
        {
            Engine.AudioManager.System.playSound(NativeSound, group, false, out _nativeChannel);
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        public void Play()
        {
            Engine.AudioManager.System.playSound(NativeSound, default, false, out _nativeChannel);
        }
        #endregion

        #region Disposing methods
        public void Dispose()
        {
            if (!_hasDisposed)
            {

                NativeSound.release();
                // free the streamed memory!
                if (_buffer != null)
                {
                    _bufferHandle.Free();
                }
                _buffer = null;
                
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _hasDisposed = true;
            }
        }

        ~StreamedSound()
        { 
                Dispose();
        }


        #endregion
    }
}
