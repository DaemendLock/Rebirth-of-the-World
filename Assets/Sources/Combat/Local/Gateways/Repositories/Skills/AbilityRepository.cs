using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class AbilityRepository : IAbilityRepository
    {
        private readonly Dictionary<AbilityKey, AbilityData> _values;

        public AbilityRepository()
        {
            _values = new();
        }

        public void Create(Ability ability)
        {
            AbilityData data = new(ability);
            _values[new(ability.Owner, ability.SkillId)] = data;
        }

        public Ability Get(AbilityKey key)
        {
            if (_values.TryGetValue(key, out AbilityData data) == false)
            {
                throw new System.InvalidOperationException();
            }

            return data.ToAbility(key.Owner, key.Skill);
        }

        public void Update(Ability value) => _values[new(value.Owner, value.SkillId)] = new(value);

        public void Delete(AbilityKey key) => _values.Remove(key);
    }
}
