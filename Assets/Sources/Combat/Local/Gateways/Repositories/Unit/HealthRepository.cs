using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class HealthRepository : IHealthRepository
    {
        private readonly Dictionary<EntityId, HealthData> _values;

        private readonly IAttributesRepository _attributesRepository;

        public HealthRepository(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;

            _values = new();
        }

        public void Create(Health value) => _values.Add(value.Id, new(value.CurrentHealth, value.DefaultHealth));

        public void Delete(EntityId id) => _values.Remove(id);

        public Health Get(EntityId id)
        {
            if (_values.TryGetValue(id, out HealthData value) == false)
            {
                return new(id, default, default, default);
            }

            Attributes attributes = _attributesRepository.Get(id);

            return new(id, value.CurrentHealth, value.DefaultHealth + attributes.GetMaxHealthBonus(), value.DefaultHealth);
        }

        public void Update(Health value) => _values[value.Id] = new(value.CurrentHealth, value.DefaultHealth);
    }
}
