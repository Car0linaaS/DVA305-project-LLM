namespace RBTest
{
    public class TestInputSequences
    {
        public static IEnumerable<int[]> InputSequences
        {
            get {
                // Sequence only containing one element
                yield return new int[] { 5 };
                // Sorted sequence
                yield return new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                // Backward sorted sequence with gaps (used by successor and predeccessor tests)
                yield return new int[] { 10, 8, 7, 6, 5, 4, 3, 1 };
                // Randomized, large sequence

                Random rand = new Random(1);

                int largeArraySize = 1000000;
                int[] largesequence = new int[largeArraySize];

                // Fill the array with randomized values
                for (int i = 0; i < largeArraySize; i++)
                {
                    largesequence[i] = rand.Next(1000); // Generates a random integer
                }
                yield return largesequence;
            }

        }
    }
}
