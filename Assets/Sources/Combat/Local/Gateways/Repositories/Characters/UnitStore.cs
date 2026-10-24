using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class UnitStore : IUnitStore
    {
        private readonly Dictionary<UnitId, UnitContext> _values;

        public UnitStore()
        {
            _values = new();
        }

        public void Create(UnitContext unit) => _values.Add(unit.Id, unit);

        public UnitContext Get(UnitId id) => _values[id];

        public bool TryGet(UnitId id, out UnitContext result) => _values.TryGetValue(id, out result);

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
