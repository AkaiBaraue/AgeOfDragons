// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityQueue.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A queue that is based on priority.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Helper_Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A queue that is based on priority.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of value that is to be saved.
    /// </typeparam>
    /// <typeparam name="TPriority">
    /// The type of priority that is to be linked to the TValue.
    /// </typeparam>
    public class PriorityQueue<TValue, TPriority> where TPriority : IComparable
    {
        /// <summary>
        /// A SortedDictionary that holds the elements of the queue.
        /// </summary>
        private SortedDictionary<TPriority, Queue<TValue>> dict = new SortedDictionary<TPriority, Queue<TValue>>();

        /// <summary>
        /// Gets the amount of elements in the priorityqueue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the queue is empty or not.
        /// </summary>
        public bool Empty
        {
            get { return this.Count == 0; }
        }

        /// <summary>
        /// Adds an element to the queue with a default priority.
        /// </summary>
        /// <param name="val"> The element to be added to the queue. </param>
        public void Enqueue(TValue val)
        {
            this.Enqueue(val, default(TPriority));
        }

        /// <summary>
        /// Adds an element to the queue with a certain priority.
        /// </summary>
        /// <param name="val"> The element to be added to the queue. </param>
        /// <param name="pri"> The pri of the element. </param>
        public void Enqueue(TValue val, TPriority pri)
        {
            ++this.Count;
            if (!this.dict.ContainsKey(pri))
            {
                this.dict[pri] = new Queue<TValue>();
            }

            this.dict[pri].Enqueue(val);
        }

        /// <summary>
        /// Gets the element with the highest priority.
        /// </summary>
        /// <returns>
        /// The element with the highest priority.
        /// </returns>
        public TValue Dequeue()
        {
            --this.Count;
            var item = this.dict.Last();
            if (item.Value.Count == 1)
            {
                this.dict.Remove(item.Key);
            }

            return item.Value.Dequeue();
        }
    }
}