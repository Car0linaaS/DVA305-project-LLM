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
            stream.Write(bytes, 0, bytes.Length);
        }

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

        // Convert 32 bits to int
        public int ReadInt32()
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

        // Read one bit from the buffer
        public bool ReadBit()
        {
            // If all bits in the buffer have been read, refill it
            if (bitsRemaining == 0)
            {
                int nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    throw new EndOfStreamException();
                }
                buffer = (byte)nextByte;
                bitsRemaining = 8;
            }

            // Get the most significant bit (msb) from the buffer by bitwise ANDing (&) the buffer and binary with msb set to one
            // If the AND result is not 0, the msb of the buffer is one, and we set bit to true
            // If the result is 0, bit will be seet to false
            bool bit = (buffer & 0b10000000) != 0;

            // Shift buffer bits to the left by one, (discarding the msb) to prepare for the next bit 
            buffer <<= 1;
            bitsRemaining--;

            return bit;
        }

        // Close the stream when disposing
        public void Dispose()
        {
            stream.Close();
        }
    }


}