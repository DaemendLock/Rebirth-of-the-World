using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IItemOwnerRepository
    {
        void Create(ItemOwner itemOwner);
        bool TryGet(UnitId id, out ItemOwner itemOwner);
        void Delete(ItemOwner itemOwner);
    }
}
