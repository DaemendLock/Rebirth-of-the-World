using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHitboxRepository
    {
        void Create(Hitbox hitbox);
        Hitbox Get(HitboxId id);
        void Update(Hitbox hitbox);
        void Delete(HitboxId id);
    }
}
