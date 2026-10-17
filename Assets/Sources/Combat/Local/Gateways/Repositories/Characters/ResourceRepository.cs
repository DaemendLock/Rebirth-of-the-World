using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class ResourceRepository : IResourceOwnerRepository
    {
        private readonly Dictionary<UnitId, ResourceOwner> _values = new();

        public void Create(ResourceOwner value) => _values.Add(value.Id, value);

        public ResourceOwner Get(UnitId entityId) => _values[entityId];

        public void Delete(UnitId entityId) => _values.Remove(entityId);
    }
}
