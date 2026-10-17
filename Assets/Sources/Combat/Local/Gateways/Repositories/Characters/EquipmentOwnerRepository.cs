using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class EquipmentOwnerRepository : IItemOwnerRepository
    {
        private readonly Dictionary<UnitId, ItemOwner> _values = new();

        public void Create(ItemOwner value) => _values.Add(value.Id, value);

        public void Delete(ItemOwner itemOwner) => _values.Remove(itemOwner.Id);

        public bool TryGet(UnitId id, out ItemOwner itemOwner) => _values.TryGetValue(id, out itemOwner);
    }
}
