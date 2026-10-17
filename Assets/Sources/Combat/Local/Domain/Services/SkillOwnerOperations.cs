using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Characters;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Services.Skills
{
    public sealed class ItemOwnerOperations
    {
        private readonly IItemOwnerRepository _repository;

        public void Equip(ItemOwner itemOwner, ItemSlot itemInfo, EquipmentSlotType slot)
        {
            ItemSlot?[] items = new ItemSlot?[itemOwner.Items.Length];
            itemOwner.Items.CopyTo(items);
            items[(int)slot] = itemInfo;
            _repository.Update(new(itemOwner.Id, items));
        }
    }
}
