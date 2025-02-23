namespace Huffman
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

        // Write int32 to stream
        public void WriteInt32(int num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            stream.Write(bytes);
        }

        // Write byte to stream
        public void WriteByte(byte b)
        {
            stream.WriteByte(b);
        }

        // Write one bit to buffer, output one byte to stream for each 8 bits
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

        // Flush any remaining bits to the stream
        public void Flush()
        {
            if (bitsWritten > 0)
            {
                stream.WriteByte(buffer);
                buffer = 0;
                bitsWritten = 0;
            }
        }

        // Flush and close the stream when disposing
        public void Dispose()
        {
            Flush();
            stream.Close();
        }
    }

    class BitStreamReader : IDisposable
    {
        private readonly FileStream stream;
        private byte buffer;
        private int bitsRemaining;

        public BitStreamReader(string filePath)
        {
            stream = new FileStream(filePath, FileMode.Open);
            buffer = 0;
            bitsRemaining = 0;
        }

        public int ReadInt32()
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                int nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    throw new System.IO.EndOfStreamException();
                }
                bytes[i] = (byte)nextByte;
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        public bool ReadBit()
        {
            if (bitsRemaining == 0)
            {
                int nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    throw new System.IO.EndOfStreamException();
                }
                buffer = (byte)nextByte;
                bitsRemaining = 8;
            }

            bool bit = (buffer & 0b10000000) != 0;

            buffer <<= 1;
            bitsRemaining--;

            return bit;
        }

        public void Dispose()
        {
            stream.Close();
        }
    }
}