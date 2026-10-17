using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

using System;

namespace Combat.Domain.Entities
{
    public readonly struct ItemOwner
    {
        private readonly ItemSlot?[] _items;

        public ItemOwner(UnitId id, ReadOnlySpan<ItemSlot?> items)
        {
            Id = id;
            _items = items.ToArray();
        }

        public readonly UnitId Id { get; }

        public readonly ReadOnlySpan<ItemSlot?> Items => _items;

        public void Equip(ItemSlot itemInfo, EquipmentSlotType slot) => _items[(int)slot] = itemInfo;
    }
}
