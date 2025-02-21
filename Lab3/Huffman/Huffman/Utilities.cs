﻿using System.Reflection;
using System.Text;

namespace Huffman
{
    public static class Utilities
    {
        // Convert byte to binary string
        public static string ByteToBin(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0'); // Fixed: corrected method call and padding character
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


        // String to binary string
        public static string StrToBinStr(string str)
        {
            StringBuilder binStr = new(); // Fixed: used StringBuilder methods correctly
            foreach (var c in str)
            {
                byte asciiByte = Encoding.ASCII.GetBytes(new char[] { c })[0];
                string binaryString = Convert.ToString(asciiByte, 2).PadLeft(8, '0');
                binStr.Append(binaryString); // Fixed: Append instead of Add
            }

            return binStr.ToString(); // Fixed: return string from StringBuilder
        }
    }
}
