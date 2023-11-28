using System.Collections.Generic;

namespace Utils.DataStructure
{
    public class TwinReadWriteQueue<T>
    {
        private readonly Queue<T> _firstQueue;
        private readonly Queue<T> _secondQueue;
        private bool _readFirst;

        public TwinReadWriteQueue()
        {
            _firstQueue = new Queue<T>();
            _secondQueue = new Queue<T>();
        }

        public int Count => _firstQueue.Count + _secondQueue.Count;

        public void Enqueue(T item) => (_readFirst ? _secondQueue : _firstQueue).Enqueue(item);

        public void Enqueue(T item, bool reverse) => (_readFirst ^ reverse ? _secondQueue : _firstQueue).Enqueue(item);

        public T Dequeue() => GetActiveReadQueue(false).Dequeue();

        public T Peek() => GetActiveReadQueue(false).Peek();

        public void Clear()
        {
            _firstQueue.Clear();
            _secondQueue.Clear();
        }

        public void Flip() => _readFirst = !_readFirst;

        private Queue<T> GetActiveReadQueue(bool reverse)
        {
            Queue<T> result = _readFirst ^ reverse ? _firstQueue : _secondQueue;

            if (result.Count > 0)
            {
                return result;
            }

            return _readFirst ^ reverse ? _secondQueue : _firstQueue;
        }
    }
}
