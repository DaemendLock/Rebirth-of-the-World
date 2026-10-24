using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class UnitStore : IUnitStore
    {
        private const int UnitsPerChunk = 32;

        private readonly Dictionary<UnitId, int> _indices;
        private readonly Stack<int> _free;

        private readonly List<UnitContext[]> _chunks;
        private int _nextIndex;

        public UnitStore()
        {
            _indices = new();
            _free = new();

            _chunks = new() { new UnitContext[UnitsPerChunk] };
            _nextIndex = 0;
        }

        public void Create(UnitContext unit)
        {
            if (_indices.ContainsKey(unit.Id))
            {
                throw new InvalidOperationException($"Unit {unit.Id} already exists.");
            }

            int index = _free.Count > 0 ? _free.Pop() : _nextIndex++;

            if (index / UnitsPerChunk == _chunks.Count)
            {
                _chunks.Add(new UnitContext[UnitsPerChunk]);
            }

            GetSlot(index) = unit;
            _indices.Add(unit.Id, index);
        }

        public ref UnitContext Get(UnitId id) => ref GetSlot(_indices[id]);

        public void Delete(UnitId id)
        {
            if (_indices.TryGetValue(id, out int index) == false)
            {
                return;
            }

            _indices.Remove(id);
            GetSlot(index) = default;
            _free.Push(index);
        }

        private ref UnitContext GetSlot(int index) => ref _chunks[index / UnitsPerChunk][index % UnitsPerChunk];
    }
}
