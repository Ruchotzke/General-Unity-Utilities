using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUnityUtils.PriorityQueue
{
    /// <summary>
    /// A simple priority queue implemented with a minheap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        /// <summary>
        /// How large is the heap initially?
        /// </summary>
        public const int INITIALSIZE = 16;

        /// <summary>
        /// The heap container.
        /// </summary>
        HeapNode<T>[] Heap;

        /// <summary>
        /// A counter used to ensure the queue uses a normal queue property when
        /// priorities are the same.
        /// </summary>
        private uint Counter = 0;

        /// <summary>
        /// The number of items in the queue.
        /// </summary>
        public int Size
        {
            get { return _size; }
        }
        int _size;

        public PriorityQueue()
        {
            Heap = new HeapNode<T>[INITIALSIZE];
            _size = 0;
        }

        /// <summary>
        /// Look at, but don't remove the minimal item in the queue.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (_size == 0) return default(T);
            return Heap[0].Value;
        }

        /// <summary>
        /// Look at the minimal priority on the queue right now.
        /// </summary>
        /// <returns></returns>
        public float PeekPriority()
        {
            if (_size == 0) return float.MinValue;
            return Heap[0].Priority;
        }

        /// <summary>
        /// Remove the minimal item from the queue.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            /* make sure we have an item in the heap */
            if(_size <= 0) return default(T);

            /* if we have only one item, just delete it */
            if(_size == 1)
            {
                _size -= 1;
                return Heap[0].Value;
            }

            /* we have multiple items, we need to reheapify afterwards */
            HeapNode<T> root = Heap[0];
            Heap[0] = Heap[_size - 1];
            _size--;
            Heapify(0);
            return root.Value;
        }

        /// <summary>
        /// Add a new value into the heap with a given
        /// initial priority.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        /// <param name="priority">The priority of the value.</param>
        public void Enqueue(T value, float priority)
        {
            /* Ensure we have enough room for the new item */
            EnsureCapacity();

            /* Add the new item to the end of the array */
            int index = _size;
            Heap[index] = new HeapNode<T>(value, priority, Counter++);
            _size++;

            /* Reheapify this heap */
            while(((Heap[Parent(index)].Priority > priority) || (Heap[Parent(index)].Priority == priority && Heap[Parent(index)].Count > Heap[index].Count)) && index > 0)
            {
                /* percolate up */
                HeapNode<T> temp = Heap[index];
                Heap[index] = Heap[Parent(index)];
                Heap[Parent(index)] = temp;
                index = Parent(index);
            }
        }
        
        /// <summary>
        /// Attempt to decrease the key's priority to a smaller priority.
        /// Throws an exception if not found.
        /// </summary>
        /// <param name="value">The value to update.</param>
        /// <param name="smallerPriority">The priority to reduce to.</param>
        public void DecreaseKey(T value, float smallerPriority)
        {
            /* Find the object */
            for(int i = 0; i < _size; i++)
            {
                if (Heap[i].Value.Equals(value))
                {
                    /* Check priority. */
                    if (Heap[i].Priority < smallerPriority)
                    {
                        throw new Exception("Improper priority used for DecreaseKey.");
                    }

                    /* Update and reheapify */
                    Heap[i].Priority = smallerPriority;

                    while (Heap[i].Priority < Heap[Parent(i)].Priority && i > 0)
                    {
                        /* percolate up */
                        HeapNode<T> temp = Heap[i];
                        Heap[i] = Heap[Parent(i)];
                        Heap[Parent(i)] = temp;
                        i = Parent(i);
                    }

                    return; //because we changed i just return
                }
            }

            /* Object not found */
            throw new Exception("Heap value not found.");
        }

        /// <summary>
        /// Get the current priority of a key inside of this heap.
        /// </summary>
        /// <param name="value">The value to check the priority of.</param>
        /// <returns>The priority of the supplied node.</returns>
        public float GetPriority(T value)
        {
            /* Find the object */
            for (int i = 0; i < _size; i++)
            {
                if (Heap[i].Value.Equals(value))
                {
                    return Heap[i].Priority;
                }
            }

            /* Object not found */
            throw new Exception("Heap value not found.");
        }

        /// <summary>
        /// Checks whether a given value is present in this heap.
        /// </summary>
        /// <param name="value">The value to be searched for.</param>
        /// <returns>True if found, else false.</returns>
        public bool Contains(T value)
        {
            /* Find the object */
            for (int i = 0; i < _size; i++)
            {
                if (Heap[i].Value.Equals(value))
                {
                    return true;
                }
            }

            /* Object not found */
            return false;
        }

        /// <summary>
        /// Make sure the heap can fit the new element.
        /// If the heap is too small, resize it.
        /// </summary>
        void EnsureCapacity()
        {
            if(_size + 1 >= Heap.Length)
            {
                /* Copy into a new array */
                HeapNode<T>[] copy = new HeapNode<T>[_size * 2];
                Array.Copy(Heap, copy, Heap.Length);

                /* Set the new array as the old */
                Heap = copy;
            }
        }

        /// <summary>
        /// Get the parent index of an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int Parent(int index)
        {
            return (index - 1) / 2;
        }

        /// <summary>
        /// Get the index of an index's left child.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int Left(int index)
        {
            return 2 * index + 1;
        }

        /// <summary>
        /// Get the index of an index's right child.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int Right(int index)
        {
            return 2 * index + 2;
        }

        /// <summary>
        /// Heapify the heap array starting from the given index.
        /// </summary>
        /// <param name="index"></param>
        void Heapify(int index)
        {
            /* Get useful indices */
            int left = Left(index);
            int right = Right(index);

            /* Get the new smallest root */
            int min = index;
            if(left < _size && ((Heap[left].Priority < Heap[min].Priority) || (Heap[left].Priority == Heap[min].Priority && Heap[left].Count < Heap[min].Count)))
            {
                min = left;
            }
            if(right < _size && ((Heap[right].Priority < Heap[min].Priority) || (Heap[right].Priority == Heap[min].Priority && Heap[right].Count < Heap[min].Count)))
            {
                min = right;
            }

            /* If the heap is out of order, swap elements and reheapify the subtree */
            if(min != index)
            {
                HeapNode<T> temp = Heap[min];
                Heap[min] = Heap[index];
                Heap[index] = temp;
                Heapify(min);
            }
        }

        /// <summary>
        /// The container for a value within the heap.
        /// NOTE: Equivalent values imply equivalent objects.
        /// </summary>
        /// <typeparam name="H"></typeparam>
        public class HeapNode<H>
        {
            public H Value;
            public float Priority;
            public uint Count;

            public HeapNode(H value, float priority, uint count)
            {
                Value = value;
                Priority = priority;
                Count = count;
            }

            public override bool Equals(object obj)
            {
                return obj is HeapNode<H> node &&
                       EqualityComparer<H>.Default.Equals(Value, node.Value);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Value);
            }
        }
    }
}