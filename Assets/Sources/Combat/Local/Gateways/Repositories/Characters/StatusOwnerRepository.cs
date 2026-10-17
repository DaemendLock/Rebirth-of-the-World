using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class StatusOwnerRepository : IStatusOwnerRepository
    {
        private readonly Dictionary<UnitId, StatusOwner> _values;

        public StatusOwnerRepository()
        {
            _values = new();
        }

        public void Create(StatusOwner statusOwner) => _values.Add(statusOwner.Id, statusOwner);

        public void Delete(UnitId id) => _values.Remove(id);

        public StatusOwner Get(UnitId id) => _values[id];

        public bool TryGet(UnitId id, out StatusOwner statusOwner) => _values.TryGetValue(id, out statusOwner);
    }
}
