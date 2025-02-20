
namespace PriorityQueues
{
	class MainClass
	{
        public static void Main(string[] args)
		{
            Console.WriteLine("Queue Size\t| Best Enqueue\t| Worst Enqueue\t| Average Enqueue\t| Best Dequeue\t| Worst Dequeue\t| Average Dequeue");

            for(var size = Math.Pow(2,8); size < Math.Pow(2, 22); size*=2)
            {
                TestPriorityQueue(size, out int bestEnqueue, out int worstEnqueue, out double avgEnqueue, out int bestDequeue, out int worstDequeue, out double avgDequeue);
                Console.WriteLine($"{size}\t\t| {bestEnqueue}\t\t| {worstEnqueue}\t\t| {avgEnqueue:F2}\t\t\t| {bestDequeue}\t\t| {worstDequeue}\t\t| {avgDequeue:F2}");
            }
        }

        // Creates a queue of size n and calculates the best, worst and average case
        // number of computational steps performed. These are based on 
        static public void TestPriorityQueue(double n, out int bestEnqueue, out int worstEnqueue, out double avgEnqueue, out int bestDequeue, out int worstDequeue, out double avgDequeue)
        {
            PriorityQueue<string, int> queue = new();
            var rand = new Random();

            int totalEnqueueSteps = 0;
            int totalDequeueSteps = 0;
            int minEnqueueSteps = int.MaxValue;
            int maxEnqueueSteps = int.MinValue;
            int minDequeueSteps = int.MaxValue;
            int maxDequeueSteps = int.MinValue;

            for (int i = 0; i < n; i++)
            {
                queue.Enqueue("", rand.Next());
                totalEnqueueSteps += queue.ComputationalSteps;
                minEnqueueSteps = Math.Min(minEnqueueSteps, queue.ComputationalSteps);
                maxEnqueueSteps = Math.Max(maxEnqueueSteps, queue.ComputationalSteps);
            }

            for (int i = 0; i < n; i++)
            {
                queue.Dequeue();
                totalDequeueSteps += queue.ComputationalSteps;
                minDequeueSteps = Math.Min(minDequeueSteps, queue.ComputationalSteps);
                maxDequeueSteps = Math.Max(maxDequeueSteps, queue.ComputationalSteps);
            }

            bestEnqueue = minEnqueueSteps;
            worstEnqueue = maxEnqueueSteps;
            avgEnqueue = (double)(totalEnqueueSteps) / n;

            bestDequeue = minDequeueSteps;
            worstDequeue = maxDequeueSteps;
            avgDequeue = (double)(totalDequeueSteps) / n;
        }
	}
}

/* ----------------------------------------------------------------------------------------------------------------------------
 * TEST RESULTS:
 * ----------------------------------------------------------------------------------------------------------------------------
 * 
 * Queue Size      | Best Enqueue  | Worst Enqueue | Average Enqueue       | Best Dequeue  | Worst Dequeue | Average Dequeue
 * 256             | 1             | 8             | 2,33                  | 1             | 8             | 6,44
 * 512             | 1             | 9             | 2,17                  | 1             | 9             | 7,40
 * 1024            | 1             | 10            | 2,31                  | 1             | 10            | 8,35
 * 2048            | 1             | 10            | 2,23                  | 1             | 11            | 9,39
 * 4096            | 1             | 11            | 2,29                  | 1             | 12            | 10,36
 * 8192            | 1             | 12            | 2,26                  | 1             | 13            | 11,36
 * 16384           | 1             | 12            | 2,29                  | 1             | 14            | 12,37
 * 32768           | 1             | 15            | 2,29                  | 1             | 15            | 13,36
 * 65536           | 1             | 16            | 2,28                  | 1             | 16            | 14,36
 * 131072          | 1             | 16            | 2,28                  | 1             | 17            | 15,36
 * 262144          | 1             | 17            | 2,29                  | 1             | 18            | 16,36
 * 524288          | 1             | 18            | 2,28                  | 1             | 19            | 17,37
 * 1048576         | 1             | 20            | 2,28                  | 1             | 20            | 18,37
 * 2097152         | 1             | 21            | 2,28                  | 1             | 21            | 19,37
 *
 * ----------------------------------------------------------------------------------------------------------------------------
 * ANALYSIS BEST/WORST CASE:
 * ----------------------------------------------------------------------------------------------------------------------------
 *
 * Enqueue and Dequeue best case: As we can see in the result table above, for all queue sizes, 
 * enqueueing or dequeueing an element takes only one computational step in the best case. 
 * This happens when the element is added to the array without any need to rearrange the heap. 
 * This shows that both Enqueue and Dequeue has a complexity of O(1) in the best case.
 * 
 * Enqueue and Dequeue worst case: As we can see in the result table above, 
 * the computational steps increase logarithmically with the size of the queue. 
 * In the worst case scenario, removing or adding an element causes a complete reshuffle of the heap,
 * I.e. "floating" an element up (Enqueue) or down (Dequeue) through each level of the hypothetical tree.
 * We know that the height of a binary tree is log(n), and the results above clearly show that for all queue sizes, 
 * the worst case scenario never requires more steps than log(size). 
 * This shows that both the Enqueue and Dequeue has a complexity of O(log n) in the worst case.
 *
 * ----------------------------------------------------------------------------------------------------------------------------
 * AVERAGE TREND ANALYSIS:
 * ----------------------------------------------------------------------------------------------------------------------------
 * 
 * For the enqueue operation, the number of computational steps stays relatively constant at around 2 when the size grows,
 * because even though it is O(log n) in the worst case, only a small part of the height is traversed 
 * on average when inserting. This is due to the nature of the queue, and the tests using randomized values. 
 * Based on the test results, using a random heap with repeated insertions, the average time complexity 
 * for Enqueue is closer to constant than logarithmic.
 * 
 * 
 * For the dequeue operation, the number of steps grows logarithmically with the number of elements in the heap, 
 * as it has to traverse a larger part of the tree to maintain the heap property. During the dequeue operation,
 * the root is swapped with the last element in the queue, which in a max-heap implementation is always a lower value. 
 * Since this is now placed in the root, it must "float" down to it's correct spot again, which is basically the
 * height of the tree. This is why based on these test results, the average time complexity for Dequeue is logarithmic.
 *
 */