using System;
using System.Collections.Generic;

namespace Combat.Common.ValueObjects
{
    public readonly struct AbilityKey : IEquatable<AbilityKey>
    {
        public readonly UnitId? Owner;
        public readonly SkillId Skill;

        public AbilityKey(UnitId? owner, SkillId skill)
        {
            Owner = owner;
            Skill = skill;
        }

        public override bool Equals(object obj) => obj is AbilityKey key && Equals(key);
        public bool Equals(AbilityKey other) => EqualityComparer<UnitId?>.Default.Equals(Owner, other.Owner) && EqualityComparer<SkillId>.Default.Equals(Skill, other.Skill);
        public override int GetHashCode() => HashCode.Combine(Owner, Skill);

        public static bool operator ==(AbilityKey left, AbilityKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AbilityKey left, AbilityKey right)
        {
            return !(left == right);
        }
    }
}
