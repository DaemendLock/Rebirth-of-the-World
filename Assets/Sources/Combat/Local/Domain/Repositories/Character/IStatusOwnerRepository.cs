using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusOwnerRepository
    {
        void Create(StatusOwner statusOwner);
        StatusOwner Get(EntityId id);
        void Update(StatusOwner statusOwner);
        void Delete(StatusOwner statusOwner);
    }
}
