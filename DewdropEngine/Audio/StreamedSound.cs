﻿using Dewdrop.Audio.Raw_FMOD;
using System;
using System.Runtime.InteropServices;

namespace Dewdrop.Audio
{
    /// <summary>
    /// Acts as a wrapper for FMOD sounds that uses an open stream for sounds to prevent them from being collected by the GC.
    /// Use this for long forms of audio such as background music and such!
    /// </summary>
    public class StreamedSound : IDisposable
    {
        // these fields are private because we don't want any interaction with the low level fmod sound systems 
        // the engine is supposed to handle the low level stuff
        private Sound _nativeSound;
        private Channel _nativeChannel;

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
                _nativeSound.getLength(out var length, TIMEUNIT.MS);
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


        internal StreamedSound(Sound sound, byte[] buffer, GCHandle bufferHandle)
        {
            _nativeSound = sound;
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
            Engine.AudioManager.System.playSound(_nativeSound, group, false, out _nativeChannel);
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        public void Play()
        {
            Engine.AudioManager.System.playSound(_nativeSound, default, false, out _nativeChannel);
        }
        #endregion

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                _nativeSound.release();
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
    }
}
