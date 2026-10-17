using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IResourceOwnerRepository
    {
        void Create(ResourceOwner resource);
        ResourceOwner Get(UnitId entityId);
        void Delete(UnitId entityId);
    }
}
