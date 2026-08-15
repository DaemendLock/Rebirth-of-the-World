using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IResourceOwnerRepository
    {
        void Create(ResourceOwner resource);
        void Update(ResourceOwner resource);
        ResourceOwner Get(UnitId entityId);
        void Delete(UnitId entityId);
    }
}
