using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IProjectileRepository
    {
        void Create(Projectile projectile);
        void Update(Projectile projectile);
        Projectile Get(ProjectileId id);
        void Delete(ProjectileId id);
    }
}
