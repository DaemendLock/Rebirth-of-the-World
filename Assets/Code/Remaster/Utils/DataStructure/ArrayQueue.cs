using System;

namespace Utils.DataStructures
{
    public class ArrayQueue<T>
    {
        private T[] _array;
        private int _front = 0;
        private int _rear = 0;

        public ArrayQueue()
        {
            _array = new T[1];
        }

        public ArrayQueue(int capacity)
        {
            _array = new T[capacity];
        }

        public int Count => _front > _rear ? _rear - _front + _array.Length : _rear - _front;
        public int Capacity => _array.Length;
        public bool Empty => _front == _rear;

        public void Enqueue(T item)
        {
            if (_front - _rear == 1 || _rear - _front == _array.Length - 1)
            {
                Grow((uint) _array.Length << 1);
            }

            _array[_rear++] = item;

            if (_rear == _array.Length)
            {
                _rear = 0;
            }
        }

        public T Dequeue()
        {
            if (_front == _rear)
            {
                throw new InvalidOperationException("Queue empty.");
            }

            T result = _array[_front];
            _array[_front++] = default;

            if (_front == _array.Length)
            {
                _front = 0;
            }

            return result;
        }

        public T Peek()
        {
            if (_front == _rear)
            {
                throw new InvalidOperationException("Queue empty.");
            }

            return _array[_front];
        }

        public void Clear()
        {
            _array = new T[1];
            _rear = 0;
            _front = 0;
        }

        private void Grow(uint capacity)
        {
            if (capacity < _array.Length)
            {
                throw new InvalidOperationException("Queue size exceed limit.");
            }

            T[] newArray = new T[capacity];
            int i = 0;

            while (Empty == false)
            {
                newArray[i++] = Dequeue();
            }

            _array = newArray;
            _front = 0;
            _rear = i;
        }
    }
}
