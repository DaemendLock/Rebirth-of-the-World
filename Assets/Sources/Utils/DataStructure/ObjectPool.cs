using System;
using System.Collections.Generic;
using Utils.DataStructures;
using Utils.Patterns.Factory;

namespace Utils.DataStructure
{
    public interface ObjectPool<T>
    {
        T Get();

        bool TryGet(out T result);

        void Put(T value);
    }

    public class SelfExpandableObjectPool<T> : ObjectPool<T>
    {
        private Factory<T> _factory;
        private ArrayQueue<T> _objects;

        public SelfExpandableObjectPool(Factory<T> factory) : this(factory, new ArrayQueue<T>(0))
        {
        }
        public SelfExpandableObjectPool(Factory<T> factory, int capacity) : this(factory, new ArrayQueue<T>(capacity))
        {
            for (int i = 0; i < capacity; i++)
            {
                _objects.Enqueue(_factory.Create());
            }
        }

        private SelfExpandableObjectPool(Factory<T> factory, ArrayQueue<T> queue)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _objects = queue;
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                for (int i = 0; i < _objects.Capacity; i++)
                {
                    _objects.Enqueue(_factory.Create());
                }
            }

            return _objects.Dequeue();
        }

        public bool TryGet(out T result)
        {
            result = Get();
            return true;
        }

        public void Put(T value)
        {
            _objects.Enqueue(value);
        }
    }

    public class FixedSizeObjectPool<T> : ObjectPool<T>
    {
        private Stack<T> _objects;
        private int _maxCount;

        public FixedSizeObjectPool(int poolSize)
        {
            _maxCount = poolSize;
            _objects = new Stack<T>(poolSize);
        }

        public T Get() => _objects.Count == 0 ? default : _objects.Pop();

        public bool TryGet(out T result)
        {
            if (_objects.Count == 0)
            {
                result = default;
                return false;
            }

            result = _objects.Pop();
            return true;
        }

        public void Put(T value)
        {
            if (_objects.Count == _maxCount)
            {
                throw new InvalidOperationException("Pool overflow");
            }

            _objects.Push(value);
        }
    }

    public class FreeObjectPool<T> : ObjectPool<T>
    {
        private Stack<T> _objects;

        public FreeObjectPool()
        {
            _objects = new Stack<T>();
        }

        public FreeObjectPool(int poolSize)
        {
            _objects= new Stack<T>(poolSize);
        }

        public T Get() => _objects.Count == 0 ? default : _objects.Pop();

        public bool TryGet(out T result)
        {
            if (_objects.Count == 0)
            {
                result = default;
                return false;
            }

            result = _objects.Pop();
            return true;
        }

        public void Put(T value) => _objects.Push(value);
    }
}
