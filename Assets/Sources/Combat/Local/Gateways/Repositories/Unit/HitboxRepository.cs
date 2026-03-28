using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class HurtableRepository : IHurtableRepository
    {
        private readonly Dictionary<HurtboxId, HurtboxData> _values = new();

        public void Create(Hurtbox value) => _values.Add(value.Id, new(value.Type, value.Owner));

        public void Delete(HurtboxId id) => _values.Remove(id);

        public Hurtbox Get(HurtboxId id)
        {
            HurtboxData data = _values[id];

            return new(id, data.Type, data.Owner);
        }

        public void Update(Hurtbox value) => _values[value.Id] = new(value.Type, value.Owner);
    }

    public class HitboxRepository : IHitboxRepository
    {
        private readonly Dictionary<HitboxId, HitboxData> _values = new();

        public void Create(Hitbox hitbox) => _values.Add(hitbox.Id, new(hitbox.Type, hitbox.Owner));

        public void Delete(HitboxId id) => _values.Remove(id);

        public Hitbox Get(HitboxId id)
        {
            if (_values.TryGetValue(id, out HitboxData data) == false)
            {
                return new(id, default, default);
            }

            return new(id, data.Type, data.Owner);
        }

        public void Update(Hitbox hitbox) => _values[hitbox.Id] = new(hitbox.Type, hitbox.Owner);
    }
}
