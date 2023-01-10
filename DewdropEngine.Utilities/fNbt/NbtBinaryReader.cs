using System.Diagnostics;
using System.Text;
using Dewdrop.Utilities.fNbt.Exceptions;

namespace Dewdrop.Utilities.fNbt;

/// <summary>
///     BinaryReader wrapper that takes care of reading primitives from an NBT stream,
///     while taking care of endianness, string encoding, and skipping.
/// </summary>
internal sealed class NbtBinaryReader : BinaryReader
{
    private const int SeekBufferSize = 8 * 1024;
    private readonly byte[] buffer = new byte[sizeof(double)];
    private readonly byte[] stringConversionBuffer = new byte[64];
    private readonly bool swapNeeded;

    private byte[] seekBuffer;


    public NbtBinaryReader([NotNull] Stream input, bool bigEndian)
        : base(input)
    {
        swapNeeded = BitConverter.IsLittleEndian == bigEndian;
    }


    [CanBeNull]
    public TagSelector Selector { get; set; }


    public NbtTagType ReadTagType()
    {
        int type = ReadByte();
        if (type < 0)
        {
            throw new EndOfStreamException();
        }

        if (type > (int)NbtTagType.LongArray)
        {
            throw new NbtFormatException("NBT tag type out of range: " + type);
        }

        return (NbtTagType)type;
    }


    public override short ReadInt16()
    {
        if (swapNeeded)
        {
            return Swap(base.ReadInt16());
        }

        return base.ReadInt16();
    }


    public override int ReadInt32()
    {
        if (swapNeeded)
        {
            return Swap(base.ReadInt32());
        }

        return base.ReadInt32();
    }


    public override long ReadInt64()
    {
        if (swapNeeded)
        {
            return Swap(base.ReadInt64());
        }

        return base.ReadInt64();
    }


    public override float ReadSingle()
    {
        if (swapNeeded)
        {
            FillBuffer(sizeof(float));
            Array.Reverse(buffer, 0, sizeof(float));
            return BitConverter.ToSingle(buffer, 0);
        }

        return base.ReadSingle();
    }


    public override double ReadDouble()
    {
        if (swapNeeded)
        {
            FillBuffer(sizeof(double));
            Array.Reverse(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }

        return base.ReadDouble();
    }


    public override string ReadString()
    {
        var length = ReadInt16();
        if (length < 0)
        {
            throw new NbtFormatException("Negative string length given!");
        }

        if (length < stringConversionBuffer.Length)
        {
            var stringBytesRead = 0;
            while (stringBytesRead < length)
            {
                var bytesToRead = length - stringBytesRead;
                var bytesReadThisTime = BaseStream.Read(stringConversionBuffer, stringBytesRead, bytesToRead);
                if (bytesReadThisTime == 0)
                {
                    throw new EndOfStreamException();
                }

                stringBytesRead += bytesReadThisTime;
            }

            return Encoding.UTF8.GetString(stringConversionBuffer, 0, length);
        }

        var stringData = ReadBytes(length);
        if (stringData.Length < length)
        {
            throw new EndOfStreamException();
        }

        return Encoding.UTF8.GetString(stringData);
    }


    public void Skip(int bytesToSkip)
    {
        if (bytesToSkip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bytesToSkip));
        }

        if (BaseStream.CanSeek)
        {
            BaseStream.Position += bytesToSkip;
        }
        else if (bytesToSkip != 0)
        {
            if (seekBuffer == null)
            {
                seekBuffer = new byte[SeekBufferSize];
            }

            var bytesSkipped = 0;
            while (bytesSkipped < bytesToSkip)
            {
                var bytesToRead = Math.Min(SeekBufferSize, bytesToSkip - bytesSkipped);
                var bytesReadThisTime = BaseStream.Read(seekBuffer, 0, bytesToRead);
                if (bytesReadThisTime == 0)
                {
                    throw new EndOfStreamException();
                }

                bytesSkipped += bytesReadThisTime;
            }
        }
    }


    private new void FillBuffer(int numBytes)
    {
        var offset = 0;
        do
        {
            var num = BaseStream.Read(buffer, offset, numBytes - offset);
            if (num == 0)
            {
                throw new EndOfStreamException();
            }

            offset += num;
        } while (offset < numBytes);
    }


    public void SkipString()
    {
        var length = ReadInt16();
        if (length < 0)
        {
            throw new NbtFormatException("Negative string length given!");
        }

        Skip(length);
    }


    [DebuggerStepThrough]
    private static short Swap(short v)
    {
        unchecked
        {
            return (short)(((v >> 8) & 0x00FF) |
                           ((v << 8) & 0xFF00));
        }
    }


    [DebuggerStepThrough]
    private static int Swap(int v)
    {
        unchecked
        {
            var v2 = (uint)v;
            return (int)(((v2 >> 24) & 0x000000FF) |
                         ((v2 >> 8) & 0x0000FF00) |
                         ((v2 << 8) & 0x00FF0000) |
                         ((v2 << 24) & 0xFF000000));
        }
    }


    [DebuggerStepThrough]
    private static long Swap(long v)
    {
        unchecked
        {
            return ((Swap((int)v) & uint.MaxValue) << 32) |
                   (Swap((int)(v >> 32)) & uint.MaxValue);
        }
    }
}