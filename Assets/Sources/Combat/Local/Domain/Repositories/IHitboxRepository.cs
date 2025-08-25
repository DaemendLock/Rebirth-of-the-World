using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public class HitboxRepository : IHitboxRepository
    {
        private readonly Dictionary<HitboxId, Hitbox> _values = new();

        public void Create(Hitbox hitbox) => _values.Add(hitbox.Id, hitbox);
        public void Delete(HitboxId id) => _values.Remove(id);
        public Hitbox Get(HitboxId id) => _values.GetValueOrDefault(id, default);
        public void Update(Hitbox hitbox) => _values[hitbox.Id] = hitbox;
    }

    public interface IHitboxRepository
    {
        void Create(Hitbox hitbox);
        Hitbox Get(HitboxId id);
        void Update(Hitbox hitbox);
        void Delete(HitboxId id);
    }
}
