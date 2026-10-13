using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusOwnerRepository
    {
        void Create(StatusOwner statusOwner);
        ref StatusOwner Get(UnitId id);
        bool TryGet(UnitId id, out StatusOwner statusOwner);
        void Delete(UnitId id);
    }
}
