using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly struct ItemSlot
    {
        public ItemInfo Info { get; }
        public int Count { get; }
        public int CurrentCharges { get; }
    }

    public readonly struct ItemInfo
    {
        private readonly ItemSkill[] _skills;
        private readonly ItemAttributeBonus[] _attributeBonus;

        public ItemType Type { get; }
        public ItemTargetSlot Slot { get; }
        public int MaxCharges { get; }
        public ReadOnlySpan<ItemSkill> Skills => _skills;
        public ReadOnlySpan<ItemAttributeBonus> Stats => _attributeBonus;
    }

    public readonly struct ItemSkill
    {
        public readonly SkillId Skill;
        public readonly ItemCostType SkillType;
    }

    public readonly struct ItemAttributeBonus
    {
        public readonly UnitAttribute Attribute;
        public readonly AttributeModifier Modification;
    }
}
