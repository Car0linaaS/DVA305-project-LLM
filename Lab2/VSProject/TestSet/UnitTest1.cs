using PriorityQueues;

namespace TestSet
{
    public class Tests
    {
        PriorityQueues.PriorityQueue<string, int> queue;
        Random rand;

        [SetUp]
        public void Setup()
        {
            rand = new Random();
            queue = new PriorityQueues.PriorityQueue<string, int>();
        }

        // Helper method to validate properties of a max-heap: parent > children
        private static bool ValidateMaxHeapProperties(PriorityQueues.PriorityQueue<string, int> q)
        {
            var array = q.HeapToArray();
            int l, r;

            for (int i = 0; i < array.Length; i++)
            {
                l = (i * 2) + 1;
                r = (i * 2) + 2;

                if (l < array.Length - 1 && array[l].Priority > array[i].Priority)
                {
                    return false;
                }

                if (r < array.Length - 1 && array[r].Priority > array[i].Priority)
                {
                    return false;
                }
            }

            return true;
        }

        // Helper method to check that an array is sorted in descending order
        static private bool IsArraySortedDescendingOrder(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] < array[i])
                {
                    return false;
                }
            }
            return true;
        }


        [Test]
        public void Count_NewQueueIsEmpty()
        {
            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void Count_QueueOneElement_CountIsOne()
        {
            queue.Enqueue("Test", 5);
            Assert.That(queue.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_QueueFiveElementsAndRemoveOne_CountIsFour()
        {
            for(int i = 0; i < 5; i++)
            {
                queue.Enqueue("Test", i);
            }
            queue.Dequeue();
            Assert.That(queue.Count, Is.EqualTo(4));
        }

        [Test]
        public void Enqueue_ReturnsHandleWithCorrectProperties()
        {
            var handle = queue.Enqueue("Apple", 22);
            Assert.That((handle.Element, handle.Priority), Is.EqualTo(("Apple", 22)));
        }

        [Test]
        public void Enqueue_QueueOneElement_PeekReturnsCorrectElement()
        {
            queue.Enqueue("EnqueueTest", 15);

            queue.TryPeek(out string str, out int prio);

            Assert.That((str, prio), Is.EqualTo(("EnqueueTest", 15)));
        }

        [Test]
        public void Enqueue_Queue100RandomElements_ValidateHeapProperties()
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            Assert.That(ValidateMaxHeapProperties(queue), Is.True);
        }

        [Test]
        public void Dequeue_DequeueFromEmptyQueue_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void Dequeue_Queue500RandomElements_DequeueWorksInSortedOrder()
        {
            for (int i = 0; i < 500; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            int[] extracted = new int[500];
            
            for(int i = 0; i < 500; i++)
            {
                var handle = queue.Dequeue();
                extracted[i] = handle.Priority;
            }

            Assert.That(IsArraySortedDescendingOrder(extracted), Is.True);
        }

        [Test]
        public void Dequeue_Queue100RandomElementsRemoveOne_ValidateHeapProperties()
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            queue.Dequeue();

            Assert.That(ValidateMaxHeapProperties(queue), Is.True);
        }

        [Test]
        public void TryDequeue_TryDequeueFromEmptyQueue_ReturnsFalse()
        {
            Assert.That(queue.TryDequeue(out _, out _), Is.False);
        }

        [Test]
        public void TryDequeue_Queue100RandomElements_TryDequeueReturnsBiggestElement()
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            queue.TryDequeue(out _, out int biggest);
            var arr = queue.HeapToArray();

            for (int i = 0; i < 99; i++)
            {
                Assert.That(arr[i].Priority, Is.LessThanOrEqualTo(biggest));
            }
        }

        [Test]
        public void Peek_Queue100RandomElements_PeekReturnsBiggestElement()
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            var arr = queue.HeapToArray();

            queue.TryPeek(out _, out int biggest);

            for(int i = 0; i < 100; i++)
            {
                Assert.That(arr[i].Priority, Is.LessThanOrEqualTo(biggest));
            }
        }

        [Test]
        public void TryPeek_PeekOnEmptyQueue_PeekReturnsFalse()
        {
            Assert.That(queue.TryPeek(out _, out _), Is.False);
        }

        [Test]
        public void UpdatePriority_Queue100RandomElements_IncreaseLastKeyAndValidateHeapProperties()
        {
            for (int i = 0; i < 99; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            var handle = queue.Enqueue("", rand.Next());

            handle.Priority *= 2;

            Assert.That(ValidateMaxHeapProperties(queue), Is.True);
        }

        [Test]
        public void UpdatePriority_Queue100RandomElements_DecreaseLastKeyAndValidateHeapProperties()
        {
            for (int i = 0; i < 99; i++)
            {
                queue.Enqueue("", rand.Next());
            }

            IPriorityQueueHandle<string, int> handle = queue.Enqueue("", rand.Next());

            handle.Priority /= 2;

            Assert.That(ValidateMaxHeapProperties(queue), Is.True);
        }
    }
}