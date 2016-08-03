using System;
using System.Threading;

namespace CoCSharp.Server.Core
{
    public class SaveQueue<T>
    {
        public SaveQueue()
        {
            _queueLock = new object();
            _queue = new T[32];
        }

        public int Count
        {
            get
            {
                return Thread.VolatileRead(ref _count);
            }
        }

        private readonly object _queueLock;
        private int _count;
        private T[] _queue;

        public void Enqueue(T element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            lock (_queueLock)
            {
                var length = _queue.Length;
                var i = 0;

                // Look for any duplicates in _queue
                // and exit if we find any.
                for (i = 0; i < length; i++)
                {
                    if (ReferenceEquals(_queue[i], element))
                    {
                        return;
                    }
                }

                // Look for the next free slot
                // and set it to specified element.
                for (i = 0; i < length; i++)
                {
                    if (_queue[i] == null)
                    {
                        _count++;
                        _queue[i] = element;
                        return;
                    }
                }

                // Resize _queue if we exceeded the length.
                Array.Resize(ref _queue, length * 2);
                _count++;
                _queue[length] = element;
            }
        }

        public T Dequeue()
        {
            lock (_queueLock)
            {
                var length = _queue.Length;

                // Look for the next non-free slot
                // and set to null, return that value.
                for (int i = 0; i < length; i++)
                {
                    var item = _queue[i];
                    if (item != null)
                    {
                        _count--;
                        _queue[i] = default(T);
                        return item;
                    }
                }

                return default(T);
            }
        }
    }
}
