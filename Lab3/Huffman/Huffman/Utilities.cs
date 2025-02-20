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

        // Gpt
        public static string BinStrToStr(string str)
        {
            string[] binarySegments = SplitBinaryString(str, 8);

            StringBuilder stringBuilder = new(); // Corrected type name to StringBuilder
            foreach (var segment in binarySegments)
            {
                int decimalValue = Convert.ToInt32(segment, 2); // Corrected method and argument order
                char character = (char)decimalValue; // Corrected conversion to char
                stringBuilder.Append(character);
            }

            return stringBuilder.ToString();
        }

        private static string[] SplitBinaryString(string binStr, int segmentLength)
        {
            int numSegments = binStr.Length / segmentLength;
            string[] segments = new string[numSegments];

            for (int i = 0; i < numSegments; i++)
            {
                segments[i] = binStr.Substring(i * segmentLength, segmentLength); // Added missing comma
            }

            return segments; // Added missing return statement
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
