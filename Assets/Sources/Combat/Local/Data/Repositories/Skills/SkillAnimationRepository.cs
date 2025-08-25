using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public class CastableRepository : ICastableRepository
    {
        private readonly Dictionary<(EntityId, int), SkillId> _values = new();

        public void Create(int slot, EntityId caster, SkillId skillId) => _values.Add((caster, slot), skillId);
        public void Delete(EntityId owner, int slot) => _values.Remove((owner, slot));
        public void Update(int slot, EntityId caster, SkillId skillId) => _values[(caster, slot)] = skillId;
        public SkillId Get(EntityId owner, int slot) => _values[(owner, slot)];
    }

    public class SkillAnimationRepository : ISkillAnimationRepository
    {
        private readonly Dictionary<SkillId, AnimationClip> _values;

        public SkillAnimationRepository()
        {
            _values = new();
        }

        public void Create(SkillId id, AnimationClip clip) => _values.Add(id, clip);

        AnimationClip ISkillAnimationRepository.Get(SkillId id) => _values.GetValueOrDefault(id, null);

        public void Update(SkillId id, AnimationClip clip) => _values[id] = clip;

        public void Delete(SkillId id) => _values.Remove(id);
    }
}
