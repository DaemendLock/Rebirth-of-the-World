using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class UnitStore : IUnitStore
    {
        private readonly Dictionary<UnitId, int> _indices;
        private readonly Stack<int> _free;

        private UnitContext[] _units;
        private int _nextIndex;

        public UnitStore()
        {
            _indices = new();
            _free = new();

            _units = new UnitContext[16];
            _nextIndex = 0;
        }

        public void Create(UnitContext unit)
        {
            if (_indices.ContainsKey(unit.Id))
            {
                throw new InvalidOperationException($"Unit {unit.Id} already exists.");
            }

            int index = _free.Count > 0 ? _free.Pop() : _nextIndex++;
            if (index == _units.Length)
            {
                Array.Resize(ref _units, _units.Length * 2);
            }

            _units[index] = unit;
            _indices.Add(unit.Id, index);
        }

        public ref UnitContext Get(UnitId id) => ref _units[_indices[id]];

        public void Delete(UnitId id)
        {
            if (_indices.TryGetValue(id, out int index) == false)
            {
                return;
            }

            _indices.Remove(id);
            _units[index] = default;
            _free.Push(index);
        }
    }
}
