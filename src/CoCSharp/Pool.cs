using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp
{
    [DebuggerDisplay("Count = {Count}")]
    internal class Pool<T>
    {
        public Pool(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();

            _buffer = new T[capacity];
        }

        private T[] _buffer;
        private int _ptr;

        public T this[int index] => _buffer[index];

        public int Count => Thread.VolatileRead(ref _ptr);

        public void Push(T obj)
        {
            lock (_buffer)
            {
                _buffer[_ptr++] = obj;

                if (_ptr == _buffer.Length)
                    Array.Resize(ref _buffer, _buffer.Length * 2);
            }
        }

        public T Pop()
        {
            lock (_buffer)
            {
                // Avoid going out of bound.
                if (_ptr <= 0)
                    return default(T);

                var value = _buffer[--_ptr];

                // Reset buffer value.
                _buffer[_ptr] = default(T);
                return value;
            }
        }
    }
}
