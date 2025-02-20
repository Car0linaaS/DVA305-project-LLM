namespace PriorityQueues
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

        // Helper methods to find children and parent nodes
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

        // Helper method for swapping nodes
        private void Swap(int i, int j)
        {
            (heap[j], heap[i]) = (heap[i], heap[j]);
        }

        // Inserts an element with the given priority and heapify up to fix potential violations
        // Returns an element that can be used to get the associated value and get/set the associated priority
        public IPriorityQueueHandle<TElement, TPriority> Enqueue(TElement element, TPriority priority)
        {
            // Reset steps
            enableAnalysisCounting = true;
            ComputationalSteps = 0;

            var newNode = new QueueElement<TElement, TPriority>(element, priority);

            heap.Add(newNode);

            int nodeIndex = HeapifyUp(LastIndex);

            return new QueueElementHandle<TElement, TPriority>(element, priority, this, nodeIndex);
        }

        // Removes and returns the top element (i.e., the one with highest priority) in the queue
        // Throws an InvalidOperationException if the queue is empty
        public IPriorityQueueHandle<TElement, TPriority> Dequeue()
        {
            // Reset steps
            enableAnalysisCounting = true;
            ComputationalSteps = 0;

            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue empty");
            }
            else
            {
                // Save root node, and then 
                var root = heap[0];

                RemoveRootAndHeapify();

                return root;
            }
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

        // Helper method to swap the root and the last element in list,
        // Remove the last node and heapify down to fix violations
        public void RemoveRootAndHeapify()
        {
            Swap(0, LastIndex);
            heap.RemoveAt(LastIndex);
            HeapifyDown(0);
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

        // Heapify-down: if the position of a node(A[i]) violates the property
        // in relation to one or both of its children - ”float down”
        // Returns the new index of the node
        private int HeapifyDown(int index)
        {
            // If counting is enabled, increase the computationalsteps
            if (enableAnalysisCounting)
            {
                ComputationalSteps++;
            }

            int left, right, biggest;

            if(LeftChild(index) < Count)
            {
                left = LeftChild(index);
                right = RightChild(index);

                // If left child is bigger, this becomes the biggest, otherwise put i as biggest
                biggest = (left < Count && compare.Compare(heap[left].Priority, heap[index].Priority) > 0) ? left : index;

                // Check if right child is bigger
                if(right < Count && compare.Compare(heap[right].Priority, heap[biggest].Priority) > 0)
                {
                    biggest = right;
                }

                // Swap places with biggest child (if exists) and propagate downwards
                if(biggest != index)
                {
                    Swap(index, biggest);
                    return HeapifyDown(biggest);
                }
            }

            return index;
        }

        // Heapify-up: if the position of a node (A[i]) violates the
        // property in relation to its parent - ”float up”
        // Returns the new index of the node
        private int HeapifyUp(int index)
        {
            // If counting is enabled, increase the computationalsteps
            if (enableAnalysisCounting)
            {
                ComputationalSteps++;
            }

            int parent;

            if(index < Count && index > 0)
            {
                parent = Parent(index);

                // If i is bigger than parent, swap places and propagate upwards
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
