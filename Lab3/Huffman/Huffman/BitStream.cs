﻿namespace Huffman
{
    internal class BitStreamWriter : IDisposable
    {
        private readonly FileStream stream;
        private byte buffer;
        private int bitsWritten;

        public BitStreamWriter(string filePath)
        {
            stream = new FileStream(filePath, FileMode.Create);
            buffer = 0;
            bitsWritten = 0;
        }

        public void WriteInt32(int num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            stream.Write(bytes);
        }

        public void WriteByte(byte b)
        {
            stream.WriteByte(b);
        }

        public void WriteBit(bool bit)
        {
            buffer |= (byte)((bit ? 1 : 0) << (7 - bitsWritten));
            bitsWritten++;

            if (bitsWritten == 8)
            {
                stream.WriteByte(buffer);
                buffer = 0;
                bitsWritten = 0;
            }
        }

        public void Flush()
        {
            if (bitsWritten > 0)
            {
                stream.WriteByte(buffer);
                buffer = 0;
                bitsWritten = 0;
            }
        }

        public void Dispose()
        {
            Flush();
            stream.Close();
        }
    }

    class BitStreamReader, IDisposable
    {
        private readonly FileStream stream;
        private byte buffer;
        private int bitsRemaining;

        BitStreamReader(filePath)
        {
            stream = new FileStream(filePath, FileMode.Create);
            buffer = 8;
            bitsRemaining = 8;
        }

        public void ReadInt32()
        {
            int[] num = new int[4];
            for (int i = 0; i < 4; i++)
            {
                num[i] = stream.ReadByte();
            }

            byte[] bytes = num.Select(i => (byte)i).ToArray();

            int intValue = BitConverter.ToInt32(bytes, 0);

            return intValue;
        }

        public bool ReadBit()
        {
            if (bitsRemaining == 0)
            {
                int nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    throw EndOfStreamException();
                }
                buffer = (byte)nextByte;
                bitsRemaining = 8;
            }

            bool bit = (buffer & 0b00000001) != 0;

            buffer >>= 1;
            bitsRemaining--;

            return bit;
        }

        public void Dispose()
        {
            stream.Close();
        }
    }


}