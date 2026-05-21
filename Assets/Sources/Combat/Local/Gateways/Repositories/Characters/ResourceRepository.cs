using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class ResourceRepository : IResourceOwnerRepository
    {
        private readonly Dictionary<UnitId, ResourceValue[]> _values = new();

        public void Create(ResourceOwner value)
        {
            _values.Add(value.Id, value.GetAll().ToArray());
        }

        public ResourceOwner Get(UnitId entityId) => new(entityId, _values[entityId]);

        public void Update(ResourceOwner resource)
        {
            ResourceValue[] oldValue = _values[resource.Id];

            System.ReadOnlySpan<ResourceValue> newValue = resource.GetAll();

            if (newValue.Length != oldValue.Length)
            {
                _values[resource.Id] = newValue.ToArray();
                return;
            }

            newValue.CopyTo(oldValue);
        }

        public void Delete(UnitId entityId) => _values.Remove(entityId);
    }
}
