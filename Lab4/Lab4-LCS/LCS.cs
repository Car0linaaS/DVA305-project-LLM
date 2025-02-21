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
            if (string1.Length <= string2.Length)
            {
                shorterString = string1;
                longerString = string2;
            }
            int shorterLength = shorterString.Length; // Changed float to int
            int longerLength = longerString.Length;
            int[] tableArray = new int[shorterLength + 1]; // Fixed array initialization

            for (int i = 1; i <= longerLength; i++) // Fixed missing semicolon in loop
            {
                int prevRowAndColumnValue = 0;
                for (int j = 1; j <= shorterLength; j++) // Changed `<` to `<=` for correct indexing
                {
                    int currentValueBeforeOverwritten = tableArray[j]; // Fixed incorrect indexing from `i` to `j`
                    if (longerString[i - 1] == shorterString[j - 1])
                    {
                        tableArray[j] = prevRowAndColumnValue + 1;
                    }
                    else
                    {
                        tableArray[j] = Math.Max(tableArray[j - 1], tableArray[j]);
                    }
                    prevRowAndColumnValue = currentValueBeforeOverwritten; // Moved outside of else block
                }
            }
            return tableArray[shorterLength]; // Fixed index to return correct LCS length
        }
        // END OF TEST AREA
    }
}