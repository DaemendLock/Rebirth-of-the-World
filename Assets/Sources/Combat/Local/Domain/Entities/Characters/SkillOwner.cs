using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct SkillOwner
    {
        public SkillOwner(UnitId id, ReadOnlySpan<SkillId> skills)
        {
            Id = id;
            Skills = skills;
            Cooldowns = Span<(SkillId, float)>.Empty;
        }

        public UnitId Id { get; }

        public ReadOnlySpan<SkillId> Skills { get; }

        public Span<(SkillId, float)> Cooldowns { get; }

        public int SkillCount => Skills.Length;

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

        public ReadOnlySpan<SkillId> GetAll() => Skills;

        public float GetCooldown(SkillId skillId)
        {
            foreach (var value in Cooldowns)
            {
                if (value.Item1 != skillId)
                {
                    continue;
                }

                return value.Item2;
            }

            return 0f;
        }

        public void SetCooldown(SkillId skillId, float cooldown)
        {
            var values = Cooldowns;

            for (int i = 0; i < Cooldowns.Length; i++)
            {
                if (values[i].Item1 != skillId)
                {
                    continue;
                }

                values[i].Item2 = cooldown;
            }


        }
    }
}
