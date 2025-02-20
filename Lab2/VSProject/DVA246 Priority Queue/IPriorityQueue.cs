using System;

/* do not change the content */

namespace PriorityQueues
{
	public interface IPriorityQueue<TElement, TPriority>
	{
		// returns the number of elements in the queue
		int Count { get; }

		// inserts an element with the given priority
		// returns an element that can be used to get the associated value and get/set the associated priority
		IPriorityQueueHandle<TElement, TPriority> Enqueue(TElement element, TPriority priority);

		// removes and returns the top element (i.e., the one with highest priority) in the queue
		// throws an InvalidOperationException if the queue is empty
		IPriorityQueueHandle<TElement, TPriority> Dequeue();

		// removes the top element from the queue passing out the element and the priority (through the out parameters)
		// returns true if the queue is not empty and false otherwise
		// in case the queue is empty, element and priority are given default values
		bool TryDequeue(out TElement element, out TPriority priority);

		// returns the top element in the queue without removing it passing out the element and the priority using the out parameters
		// returns true if the queue is not empty and false otherwise
		// in case the queue is empty, element and priority are given default values
		bool TryPeek(out TElement element, out TPriority priority);
	}

	public interface IPriorityQueueHandle<TElement, TPriority>
	{
		// get the element
		TElement Element { get; }

		// gets or sets the priority
		// changing the priority requires the associated priority queue to be reprioritized
		TPriority Priority { get; set; }
	}

}
