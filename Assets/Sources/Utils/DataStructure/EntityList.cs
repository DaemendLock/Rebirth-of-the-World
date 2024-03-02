using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utils.DataStructure
{
    public interface Entity
    {
        int Id { get; }
    }

    public class EntityList<T> : IEnumerable<T> where T : Entity
    {
        private readonly List<T> _entities;
        private readonly Queue<int> _ids;

        public EntityList() : this(0)
        {
        }

        public EntityList(int capacity)
        {
            _entities = new List<T>(capacity);
            _ids = new Queue<int>();
        }

        public int NextId
        {
            get
            {
                if (_ids.Count > 0)
                {
                    return _ids.Peek();
                }

                return _entities.Count;
            }
        }
        
        public int Count => _entities.Count - _ids.Count;

        public T Get(int id) => _entities[id] ?? throw new System.InvalidOperationException();
        

        public void Add(T value)
        {
            if(value == null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            if(value.Id != _entities.Count && _entities[value.Id] != null)
            {
                throw new System.InvalidOperationException("Entity Id overlap");
            }

            if (_ids.Count == 0)
            {
                _entities.Add(value);
                return;
            }

            int id = _ids.Dequeue();
            _entities[id] = value;
            return;
        }

        public void Remove(int id)
        {
            if (_ids.Contains(id))
            {
                throw new System.InvalidOperationException(nameof(id));
            }

            _entities[id] = default;
        }

        public IEnumerator<T> GetEnumerator() => _entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _entities.GetEnumerator();
    }
}
