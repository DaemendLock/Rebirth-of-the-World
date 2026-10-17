using Combat.Common.Primitives;
using Combat.Local.Application.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IProjectileRepository
    {
        void Create(Projectile projectile);
        void Update(Projectile projectile);
        Projectile Get(ProjectileId id);
        void Delete(ProjectileId id);
    }
}
