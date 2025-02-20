/*************************************
      DVA246 - DoA Fortsättning
     Lab 4 - Dynamic Programming
Av Carolina Smigan och Stina Häggmark
**************************************/

using System.Text;
using Lab4_LCS;

namespace LCSTests
{
    public class Tests
    {
        LCS lcs;
        Random random;
        string string1 = "ABCDEFGHIJ";
        string string2 = "CFJBADHIGE";

        int knownLcs = 4;

        char[] charsToAdd1;
        char[] charsToAdd2;
        char[] charsToAdd3;

        StringBuilder sb1;
        StringBuilder sb2;

        int nrOfChars;

        [SetUp]
        public void Setup()
        {
            lcs = new LCS();
            random = new Random();
            nrOfChars = random.Next(10, 10000);

            // Different intervals of possible char generation
            charsToAdd1 = GetRandomStringHelperMethod(nrOfChars, 75, 100).ToCharArray();
            charsToAdd2 = GetRandomStringHelperMethod(nrOfChars, 101, 126).ToCharArray();
            charsToAdd3 = GetRandomStringHelperMethod(nrOfChars, 75, 126).ToCharArray();

            sb1 = new StringBuilder(string1.Length + charsToAdd1.Length);
            sb2 = new StringBuilder(string2.Length + charsToAdd2.Length);
        }

        [Test]
        public void GetLcsLength_TwoIdenticalStrings_ReturnsStringLength()
        {
            string stringToTest = GetRandomStringHelperMethod(nrOfChars, 32, 126);
            Assert.That(lcs.GetLcsLength(stringToTest, stringToTest), Is.EqualTo(stringToTest.Length));
        }

        [Test]
        public void GetLcsLength_OneEmptyString_ReturnsZero()
        {
            string stringToTest = GetRandomStringHelperMethod(nrOfChars, 32, 126);
            Assert.That(lcs.GetLcsLength(stringToTest, ""), Is.EqualTo(0));
        }

        [Test]
        public void GetLcsLength_StringsOfDifferentLength_ReturnsCorrectValue()
        {
            string stringToTest1 = GetRandomStringHelperMethod(nrOfChars, 32, 126);
            string stringToTest2 = stringToTest1[0..(stringToTest1.Length / 2)];
            Assert.That(lcs.GetLcsLength(stringToTest1, stringToTest2), Is.EqualTo(stringToTest2.Length));
        }

        [Test]
        public void GetLcsLength_StringWithRepeatingCharacter_ReturnsCorrectValue()
        {
            string stringToTest1 = "AAAAA";
            string stringToTest2 = "ABACA";
            Assert.That(lcs.GetLcsLength(stringToTest2, stringToTest1), Is.EqualTo(3));
        }

        [Test]
        public void GetLcsLength_StringWithRepeatingCharacter_ReturnsCorrectValue2()
        {
            string stringToTest1 = "AAAAA";
            string stringToTest2 = "ABBBB";
            Assert.That(lcs.GetLcsLength(stringToTest2, stringToTest1), Is.EqualTo(1));
        }

        /***** TESTS TO SEE HOW LCS CHANGES WHEN NEW CHACTERS ARE INJECTED INTO KNOWN STRINGS *****/
        /*Randomized characters are added first, last and in between chars of known strings*/
        /*If the same characters are added in both strings, the LCS should increase*/
        /*If different characters are added in both strings, the LCS should remain the same*/

        [Test]
        public void GetLcsLength_AddMatchingCharactersLastInString_ReturnsKnownLcsPlusNumberOfChars()
        {
            sb1.Append(string1);
            sb2.Append(string2);
            for (int i = 0; i < charsToAdd3.Length; i++)
            {
                sb1.Append(charsToAdd3[i]);
                sb2.Append(charsToAdd3[i]);
            }

            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.EqualTo(knownLcs + charsToAdd3.Length));
        }

        [Test]
        public void GetLcsLength_AddNonMatchingCharactersLastInString_ReturnsKnownLcs()
        {
            sb1.Append(string1);
            sb2.Append(string2);

            for (int i = 0; i < nrOfChars; i++)
            {
                sb1.Append(charsToAdd1[i]);
                sb2.Append(charsToAdd2[i]);
            }

            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.EqualTo(knownLcs));
        }

        [Test]
        public void GetLcsLength_AddMatchingCharactersFirstInString_ReturnsKnownLcsPlusNumberOfChars()
        {
            for (int i = 0; i < charsToAdd3.Length; i++)
            {
                sb1.Append(charsToAdd3[i]);
                sb2.Append(charsToAdd3[i]);
            }

            sb1.Append(string1);
            sb2.Append(string2);

            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.EqualTo(knownLcs + charsToAdd3.Length));
        }

        [Test]
        public void GetLcsLength_AddNonMatchingCharactersFirstInString_ReturnsKnownLcs()
        { 
            for (int i = 0; i < nrOfChars; i++)
            {
                sb1.Append(charsToAdd1[i]);
                sb2.Append(charsToAdd2[i]);
            }

            sb1.Append(string1);
            sb2.Append(string2);

            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.EqualTo(knownLcs));
        }

        // If the generated string is less than the LCS for the hardcoded strings (4) then the result will be 4 AKA knownLcs
        // Else it will be the length of the generated string
        [Test]
        public void GetLcsLength_AddMatchingCharactersInTheMiddleOfString_ReturnsKnownLcsOrNumberOfChars()
        {
            int j = 0;
            int k = 0;

            for (int i = 0; i < nrOfChars + 10; i++)
            {
                if(i % 2 == 0 && j < 10 || k >= nrOfChars)
                {
                    sb1.Append(string1[j]);
                    sb2.Append(string2[j]);
                    j++;
                }
                else
                {
                    sb1.Append(charsToAdd3[k]);
                    sb2.Append(charsToAdd3[k]);
                    k++;
                }
            }
            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.AnyOf(nrOfChars, knownLcs));
        }

        [Test]
        public void GetLcsLength_AddNonMatchingCharactersInTheMiddleOfString_ReturnsKnownLcs()
        {
            int j = 0;
            int k = 0;

            for (int i = 0; i < nrOfChars + 10; i++)
            {
                if (i % 2 == 0 && j < 10 || k >= nrOfChars)
                {
                    sb1.Append(string1[j]);
                    sb2.Append(string2[j]);
                    j++;
                }
                else
                {
                    sb1.Append(charsToAdd1[k]);
                    sb2.Append(charsToAdd2[k]);
                    k++;
                }
            }

            Assert.That(lcs.GetLcsLength(sb1.ToString(), sb2.ToString()), Is.EqualTo(knownLcs));
        }

        /***** HELPER METHOD TO GENERATE RANDOM STRING *****/
        private string GetRandomStringHelperMethod(int nrOfChars, int min, int max)
        {
            StringBuilder sb = new StringBuilder(nrOfChars);

            for(int i = 0; i < nrOfChars; i++)
            {
                sb.Append((char)random.Next(min, max));
            }
            return sb.ToString();
        }
    }
}