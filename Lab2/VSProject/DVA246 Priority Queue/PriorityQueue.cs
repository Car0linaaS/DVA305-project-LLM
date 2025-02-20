﻿namespace PriorityQueues
{
    public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
    {
        // Priority Queue implemented using a max-heap
        private readonly List<QueueElement<TElement, TPriority>> heap;

        private readonly IComparer<TPriority> compare;

        // Flag to determine whether to count the computational steps (only for dequeue and enqueue)
        private bool enableAnalysisCounting = true;
        public int ComputationalSteps { get; private set; } = 0;

        // Constructors
        public PriorityQueue()
        {
            heap = new List<QueueElement<TElement, TPriority>>();
            compare = Comparer<TPriority>.Default;
        }
        public PriorityQueue(IComparer<TPriority> comparer)
        {
            heap = new List<QueueElement<TElement, TPriority>>();
            compare = comparer;
        }

        // Number of elements in queue
        public int Count => heap.Count;

        // Last index in queue
        private int LastIndex => heap.Count - 1;
        private bool IsEmpty => Count == 0;

        // START OF TEST AREA

        static private int LeftChild(int index)
        {
            return index * 2 + 1;
        }
        static private int RightChild(int index)
        {
            return index * 2 + 2;
        }
        static private int Parent(int index)
        {
            return (index - 1) / 2;
        }
        private void Swap(int i, int j)
        {
            (heap[j], heap[i]) = (heap[i], heap[j]);
        }

        // Gemini
        public IPriorityQueueHandle<TElement, TPriority> Dequeue()
        {
            enableAnalysisCounting = true;
            ComputationalSteps = 0;
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue empty");
            }
            else
            {
                var root = heap[0];
                RemoveRootAndHepify();
                return root;
            }
        }

        public void RemoveRootAndHeapify()
        {
            Swap(0, LastIndex);
            heap.RemoveAt(LastIndex);
            LastIndex--; // Decrement LastIndex
            HeapifyDown(0);
        }

        private int HeapifyDown(int index)
        {
            if (enableAnalysisCounting)
            {
                ComputationalSteps++;
            }
            int left = LeftChild(index);
            int right = RightChild(index);
            int largest = index;

            if (left < Count && compare.Compare(heap[left].Priority, heap[largest].Priority) < 0)
            {
                largest = left;
            }

            if (right < Count && compare.Compare(heap[right].Priority, heap[largest].Priority) < 0)
            {
                largest = right;
            }

            if (largest != index)
            {
                Swap(index, largest);
                return HeapifyDown(largest);
            }
            return index;
        }

        // STOP OF TEST AREA


        // Inserts an element with the given priority and heapify up to fix potential violations
        // Returns an element that can be used to get the associated value and get/set the associated priority
        public IPriorityQueueHandle<TElement, TPriority> Enqueue(TElement element, TPriority priority)
        {
            enableAnalysisCounting = true;
            ComputationalSteps = 0;
            var newNode = new QueueElement<TElement, TPriority>(element, priority);
            heap.Add(newNode);
            int nodeIndex = HeapifyUp(LastIndex);
            return new QueueElementHandle<TElement, TPriority>(element, priority, this, nodeIndex);
        }

        // Removes the top element from the queue passing out the element and the priority (through the out parameters)
        // Returns true if the queue is not empty and false otherwise
        // In case the queue is empty, element and priority are given default values
        public bool TryDequeue(out TElement element, out TPriority priority)
        {
            if (IsEmpty)
            {
                element = default;
                priority = default;
                return false;
            }

            (element, priority) = (heap[0].Element, heap[0].Priority);

            RemoveRootAndHeapify();

            return true;
        }

        // Returns the top element in the queue without removing it passing out the element and the priority using the out parameters
        // Returns true if the queue is not empty and false otherwise
        // In case the queue is empty, element and priority are given default values
        public bool TryPeek(out TElement element, out TPriority priority)
        {
            if (IsEmpty)
            {
                element = default;
                priority = default;
                return false;
            }

            (element, priority) = (heap[0].Element, heap[0].Priority);

            return true;
        }

        // Change the priority of an item
        // If priority is increased, heapify up otherwise down
        public int UpdatePriority(int i, TPriority prio)
        {
            // Since we are only analysing the enqueue and dequeue, and this method is called when creating a handle, 
            // Enqueue gets duplicate steps counted via heapify, and thus we will need to disable the counting here
            // To get accurate results for enqueue analysis

            enableAnalysisCounting = false;

            // If prio is increased, heapify up since it now might be bigger than its parents
            if (compare.Compare(prio, heap[i].Priority) > 0)
            {
                heap[i].Priority = prio;
                return HeapifyUp(i);
            }
            // If prio is decreased, heapify down since it now might be smaller than its children
            else
            {
                heap[i].Priority = prio;
                return HeapifyDown(i);
            }
        }

        // Heapify-up: if the position of a node (A[i]) violates the
        // property in relation to its parent - ”float up”
        // Returns the new index of the node
        private int HeapifyUp(int index)
        {
            if (enableAnalysisCounting)
            {
                ComputationalSteps++;
            }
            int parent;
            if(index < Count && index > 0)
            {
                parent = Parent(index);
                if (compare.Compare(heap[index].Priority, heap[parent].Priority) > 0)
                {
                    Swap(parent, index);
                    return HeapifyUp(parent);
                }
            }
            return index;
        }

        // Helper method for unit testing
        public QueueElement<TElement, TPriority>[] HeapToArray()
        {
            return heap.ToArray();
        }
    }
}
