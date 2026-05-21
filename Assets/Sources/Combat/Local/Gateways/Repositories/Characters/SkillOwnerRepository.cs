using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class SkillOwnerRepository : ISkillOwnerRepository
    {
        private readonly Dictionary<UnitId, SkillId[]> _values;

        public SkillOwnerRepository()
        {
            _values = new();
        }

        public void Create(SkillOwner value)
        {
            SkillId[] values = value.GetAll().ToArray();
            _values[value.Id] = values;
        }

        public void Update(SkillOwner value)
        {
            SkillId[] newSkills = value.GetAll().ToArray();
            _values[value.Id] = newSkills;
        }

        public SkillOwner Get(UnitId id) => new(id, _values[id].AsSpan());

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
