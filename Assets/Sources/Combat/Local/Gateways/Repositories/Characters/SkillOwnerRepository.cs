using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class SkillOwnerRepository : ISkillOwnerRepository
    {
        private readonly Dictionary<UnitId, SkillOwner> _values;

        public SkillOwnerRepository()
        {
            _values = new();
        }

        public void Create(SkillOwner value) => _values[value.Id] = value;

        public bool TryGet(UnitId id, out SkillOwner skillOwner) => _values.TryGetValue(id, out skillOwner);

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
