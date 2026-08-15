using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct SkillOwner
    {
        public SkillOwner(UnitId id, ReadOnlySpan<SkillId> skills, ReadOnlySpan<SkillCooldown> cooldowns)
        {
            Id = id;
            Skills = skills;
            Cooldowns = cooldowns;
        }

        public UnitId Id { get; }

        public ReadOnlySpan<SkillId> Skills { get; }

        public ReadOnlySpan<SkillCooldown> Cooldowns { get; }

        public SkillId? GetSkill(int index)
        {
            if (Skills.Length <= index || index < 0)
                return null;

            return Skills[index];
        }

        public bool HasSkill(SkillId value)
        {
            foreach (var skill in Skills)
            {
                if (skill == value)
                {
                    return true;
                }
            }

            return false;
        }

        public float GetCooldown(SkillId skillId)
        {
            foreach (var value in Cooldowns)
            {
                if (value.Skill != skillId)
                {
                    continue;
                }

                return value.Value;
            }

            return 0f;
        }
    }
}
