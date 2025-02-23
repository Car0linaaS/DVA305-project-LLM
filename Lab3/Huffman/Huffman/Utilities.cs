﻿using System.Reflection;
using System.Text;

namespace Huffman
{
    public static class Utilities
    {
        public static string ByteToBin(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0'); // Fixed conversion method and padding direction
        }

        public static string StrToBinStr(string str)
        {
            StringBuilder binStr = new();
            foreach (var c in str) // Fixed foreach syntax
            {
                byte asciiByte = Encoding.ASCII.GetBytes(new char[] { c })[0]; // Fixed byte extraction
                string binaryString = Convert.ToString(asciiByte, 2).PadLeft(8, '0'); // Fixed conversion
                binStr.Append(binaryString);
            }

            return binStr.ToString();
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
                int decimalValue = Convert.ToInt32(segment, 2);
                char character = Convert.ToChar(decimalValue);
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
                segments[i] = binStr.Substring(i * segmentLength, segmentLength);
            }

            return segments;
        }
    }
}