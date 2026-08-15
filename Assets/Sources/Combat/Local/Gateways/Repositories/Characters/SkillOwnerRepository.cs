using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public readonly struct SkillOwnerData
    {
        public readonly SkillId[] Skills;
        public readonly SkillCooldown[] Cooldowns;

        public SkillOwnerData(SkillId[] skills, SkillCooldown[] cooldowns)
        {
            Skills = skills;
            Cooldowns = cooldowns;
        }
    }

    public sealed class SkillOwnerRepository : ISkillOwnerRepository
    {
        private readonly Dictionary<UnitId, SkillOwnerData> _values;

        public SkillOwnerRepository()
        {
            _values = new();
        }

        public void Create(SkillOwner value)
        {
            SkillOwnerData data = new(value.Skills.ToArray(), value.Cooldowns.Length == 0 ? Array.Empty<SkillCooldown>() : value.Cooldowns.ToArray());
            _values[value.Id] = data;
        }

        public void Update(SkillOwner value)
        {
            if (_values.TryGetValue(value.Id, out var data) == false)
            {
                throw new InvalidOperationException();
            }

            SkillId[] skills = data.Skills;
            SkillCooldown[] cooldowns = data.Cooldowns;

            if (data.Skills.Length == value.Skills.Length)
            {
                value.Skills.CopyTo(skills);
            }
            else
            {
                skills = value.Skills.ToArray();
            }

            if (data.Cooldowns.Length == value.Cooldowns.Length)
            {
                value.Cooldowns.CopyTo(cooldowns);
            }
            else
            {
                cooldowns = value.Cooldowns.ToArray();
            }

            _values[value.Id] = new(skills, cooldowns);
        }

        public bool TryGet(UnitId id, out SkillOwner skillOwner)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                skillOwner = new(id, Span<SkillId>.Empty, Span<SkillCooldown>.Empty);
                return false;
            }

            skillOwner = new(id, data.Skills, data.Cooldowns);
            return true;
        }

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
