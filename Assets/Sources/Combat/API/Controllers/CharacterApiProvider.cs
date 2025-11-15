using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public class CharacterApiProvider
    {
        private readonly Dictionary<EntityId, Unit> _values;

        public CharacterApiProvider()
        {
            _values = new();
        }

        public void Register(Unit unit) => _values.Add(unit.Id, unit);

        public Unit Get(EntityId entityId) => _values.GetValueOrDefault(entityId, null);
    }
}
