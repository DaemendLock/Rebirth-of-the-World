using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusOwnerRepository
    {
        void Create(StatusOwner statusOwner);
        bool TryGet(UnitId id, out StatusOwner statusOwner);
        void Update(StatusOwner statusOwner);
        void Delete(UnitId id);
    }
}
