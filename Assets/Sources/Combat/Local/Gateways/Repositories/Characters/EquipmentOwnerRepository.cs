using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Entities.Characters;

using System.Collections.Generic;
using Combat.Local.Domain.ValueObjects;
using System;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public readonly struct EquipmentOwnerModel
    {
        public readonly ItemSlot?[] Items;

        public EquipmentOwnerModel(ItemOwner value)
        {
            Items = value.Items.ToArray();
        }

        public ItemOwner Parse(UnitId id)
        {
            return new(id, Items);
        }
    }

    public sealed class EquipmentOwnerRepository : IItemOwnerRepository
    {
        private readonly Dictionary<UnitId, EquipmentOwnerModel> _values = new();

        public void Create(ItemOwner value)
        {
            _values[value.Id] = new(value);
        }

        public void Delete(ItemOwner itemOwner) => _values.Remove(itemOwner.Id);

        public bool TryGet(UnitId id, out ItemOwner itemOwner)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                itemOwner = new(id, Array.Empty<ItemSlot?>());
                return false;
            }

            itemOwner = new(id, data.Items);
            return true;
        }

        public void Update(ItemOwner value) => value.Items.CopyTo(_values[value.Id].Items);
    }
}
