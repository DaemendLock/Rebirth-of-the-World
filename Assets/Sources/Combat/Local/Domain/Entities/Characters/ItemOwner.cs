using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities.Characters
{
    public readonly ref struct ItemOwner
    {
        public ItemOwner(UnitId id, ReadOnlySpan<ItemSlot?> items)
        {
            Id = id;
            Items = items;
        }

        public readonly UnitId Id { get; }

        public readonly ReadOnlySpan<ItemSlot?> Items { get; }
    }
}
