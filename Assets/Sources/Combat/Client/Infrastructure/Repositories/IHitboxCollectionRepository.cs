using DaeHitbox;

using Utils.Patterns.Repository;

namespace Client.Combat.Infrastructure.Repositories
{
    public interface IHitboxCollectionRepository : IRepository<IHitboxCollection, HitboxType>
    { }
}
