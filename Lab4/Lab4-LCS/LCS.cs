/*************************************
      DVA246 - DoA Fortsättning
     Lab 4 - Dynamic Programming
Av Carolina Smigan och Stina Häggmark
**************************************/

namespace Lab4_LCS
{
    public class LCS
    {
        // START OF TEST AREA
        // GPT
        public int GetLcsLength(string string1, string string2)
        {
            string shorterString = string2;
            string longerString = string1;
            if (string1.Length < string2.Length) // Fixed incorrect syntax
            {
                shorterString = string1;
                longerString = string2;
            }
            int shorterLength = shorterString.Length;
            int longerLength = longerString.Length;
            int[] tableArray = new int[shorterLength + 1]; // Fixed incorrect array initialization
            for (int i = 1; i <= longerLength; i++) // Fixed missing type for 'i'
            {
                int prevRowAndColumnValue = 0; // Changed initial value from 1 to 0 (LCS starts from 0)
                for (int j = 1; j <= shorterLength; j++) // Fixed loop boundary condition
                {
                    int currentValueBeforeOverwritten = tableArray[j];
                    if (longerString[i - 1] == shorterString[j - 1])
                    {
                        tableArray[j] = prevRowAndColumnValue + 1;
                    }
                    else
                    {
                        tableArray[j] = Math.Max(tableArray[j - 1], tableArray[j]); // Fixed incorrect function (Max instead of Min)
                    }
                    prevRowAndColumnValue = currentValueBeforeOverwritten;
                }
            }
            return tableArray[shorterLength]; // Fixed incorrect index decrement
        }

        // END OF TEST AREA
    }
}