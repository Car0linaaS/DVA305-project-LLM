using System.Reflection;
using System.Text;

namespace Huffman
{
    public static class Utilities
    {
        // Convert byte to binary string
        public static string ByteToBin(byte b)
        {
            return Convert.ToString(Convert.ToInt32(b), 2).PadLeft(8, '0');
        }

        // Convert binary string to byte
        public static byte BinToByte(string str)
        {
            return (byte)Convert.ToInt32(str, 2);
        }

        public static string BinStrToStr(string str)
        {
            string[] binarySegments = SplitBinaryString(str, 8);

            StringBuilder stringBuilder = new();
            foreach (var segment in binarySegments)
            {
                int decimalValue = Convert.ToInt32(segment, 2); // Corrected base to 2
                char character = Convert.ToChar(decimalValue);  // Corrected method name
                stringBuilder.Append(character);
            }

            return stringBuilder.ToString();
        }

        private static string[] SplitBinaryString(string binStr, int segmentLength) // Fixed method signature
        {
            int numSegments = binStr.Length / segmentLength;
            string[] segments = new string[numSegments];

            for (int i = 0; i < numSegments; i++) // Corrected loop condition
            {
                segments[i] = binStr.Substring(i * segmentLength, segmentLength); // Corrected indexing and length
            }

            return segments;
        }



        // String to binary string
        public static string StrToBinStr(string str)
        {
            StringBuilder binStr = new();
            foreach (var c in str)
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(new char[] { c });
                string binaryString = Convert.ToString(asciiBytes[0], 2).PadLeft(8, '0');
                binStr.Append(binaryString);
            }

            return binStr.ToString();
        }
    }
}
