using Combat.Common.Primitives;
using Combat.Local.Domain.Entities.Characters;

namespace Combat.Local.Domain.Repositories
{
    public interface IItemOwnerRepository
    {
        void Create(ItemOwner itemOwner);
        bool TryGet(UnitId id, out ItemOwner itemOwner);
        void Update(ItemOwner itemOwner);
        void Delete(ItemOwner itemOwner);
    }
}
