using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct SkillOwner
    {
        public SkillOwner(EntityId id, ReadOnlySpan<SkillId> skills)
        {
            Id = id;
            Skills = skills;
        }

        public EntityId Id { get; }

        public ReadOnlySpan<SkillId> Skills { get; }

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
    }
}
