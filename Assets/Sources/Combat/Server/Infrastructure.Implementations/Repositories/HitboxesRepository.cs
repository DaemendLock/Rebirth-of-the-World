using System.Collections.Generic;

using DaeHitbox;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class HitboxesRepository : IHitboxesRepository
    {
        protected readonly Dictionary<int, IHitboxCollection> Values;

        public HitboxesRepository()
        {
            Values = new();
        }

        public void Add(EntityId key, IHitboxCollection value) => Values[key.Value] = value;

        public IHitboxCollection Get(EntityId key) => Values.GetValueOrDefault(key.Value, default);

        public bool Remove(EntityId key) => Values.Remove(key.Value);
    }
}
