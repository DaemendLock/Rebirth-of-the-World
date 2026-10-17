using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IStatusOwnerRepository
    {
        void Create(StatusOwner statusOwner);
        StatusOwner Get(UnitId id);
        bool TryGet(UnitId id, out StatusOwner statusOwner);
        void Delete(UnitId id);
    }
}
