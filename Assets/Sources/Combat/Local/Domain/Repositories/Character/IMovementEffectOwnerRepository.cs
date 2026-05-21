using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IMovementEffectOwnerRepository
    {
        void Create(MovementEffectOwner value);
        MovementEffectOwner Get(UnitId unitId);
        void Delete(UnitId target);
    }
}
