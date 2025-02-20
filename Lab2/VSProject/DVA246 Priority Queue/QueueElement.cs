namespace PriorityQueues
{
    public class QueueElement<TElement, TPriority> : IPriorityQueueHandle<TElement, TPriority>
    {
        public TElement Element { get; }
        public TPriority Priority { get; set; }
        public QueueElement(TElement element, TPriority priority)
        {
            Element = element;
            Priority = priority;
        }
    }
}
