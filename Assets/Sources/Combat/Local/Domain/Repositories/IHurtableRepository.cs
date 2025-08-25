using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public class HurtableRepository : IHurtableRepository
    {
        private readonly Dictionary<HurtboxId, Hurtbox> _values = new();

        public void Create(Hurtbox hitbox) => _values.Add(hitbox.Id, hitbox);
        public void Delete(HurtboxId id) => _values.Remove(id);
        public Hurtbox Get(HurtboxId id) => _values.GetValueOrDefault(id, default);
        public void Update(Hurtbox hitbox) => _values[hitbox.Id] = hitbox;
    }

    public interface IHurtableRepository
    {
        void Create(Hurtbox hitbox);
        void Delete(HurtboxId id);
        Hurtbox Get(HurtboxId id);
    }
}
