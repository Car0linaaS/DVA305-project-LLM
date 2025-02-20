/*************************************
      DVA246 - DoA Fortsättning
     Lab 4 - Dynamic Programming
Av Carolina Smigan och Stina Häggmark
**************************************/

namespace Lab4_LCS
{
    public class LCS
    {
        // Method that returns the length of the LCS for two strings 
        public int GetLcsLength(string string1, string string2)
        {
            string shorterString = string2;
            string longerString = string1;

            // Rereference strings if needed
            if (string1.Length < string2.Length)
            {
                shorterString = string1;
                longerString = string2;
            }

            int shorterLength = shorterString.Length;
            int longerLength = longerString.Length;

            // This array has the minimum possible length in order to save memory
            // All elements will have value 0 as default
            int[] tableArray = new int[shorterLength+1];

            // Iterate through both strings and compare the "last" elements against each other
            for (int i = 1; i <= longerLength; i++)
            {
                int prevRowAndColumnValue = 0; // diagonal

                // The inner loop must correspond to the minLength
                for (int j = 1; j <= shorterLength; j++)
                {
                    // A kind of temp variable, it holds the value that is stored before it gets overwritten
                    int currentValueBeforeOverwritten = tableArray[j];

                    // if they match
                    if (longerString[i - 1] == shorterString[j - 1])
                    {
                        tableArray[j] = prevRowAndColumnValue + 1;
                    }
                    else
                    {
                        tableArray[j] = Math.Max(tableArray[j - 1], tableArray[j]);
                    }

                    prevRowAndColumnValue = currentValueBeforeOverwritten;
                }
            }
            return tableArray[tableArray.Length - 1];
        }
    }
}
