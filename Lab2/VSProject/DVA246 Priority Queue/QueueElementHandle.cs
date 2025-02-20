namespace PriorityQueues
{
    // Since changing (i.e., both lowering and increasing) the priority of an inserted element
    // usually modifies the heap, the class implementing IPriorityQueueHandle needs to keep a
    // reference to the priority queue it is part of and the index of the element.
    public class QueueElementHandle<TElement, TPriority> : IPriorityQueueHandle<TElement, TPriority>
    {
        private PriorityQueue<TElement, TPriority> Queue { get; }
        private int Index { get; set; }
        public TElement Element { get; }

        // Changing the priority requires the associated priority queue to be reprioritized
        private TPriority _priority;
        public TPriority Priority
        {
            get => _priority;
            set
            {
                Index = Queue.UpdatePriority(Index, value);
                _priority = value;
            }
        }

        // Constructor
        public QueueElementHandle(TElement element, TPriority priority, PriorityQueue<TElement, TPriority> queue, int index)
        {
            Queue = queue;
            Index = index;
            Element = element;
            Priority = priority;
        }
    }
}
