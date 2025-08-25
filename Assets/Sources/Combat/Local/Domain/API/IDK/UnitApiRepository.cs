using System.Collections.Generic;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.IDK
{
    public class UnitApiRepository
    {
        private readonly Dictionary<EntityId, Unit> _values;

        public UnitApiRepository()
        {
            _values = new();
        }

        public void Create(Unit unit) => _values.Add(unit.Id, unit);

        public Unit Get(EntityId entityId) => _values.GetValueOrDefault(entityId, null);
    }
}
