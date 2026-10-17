using Combat.Common.Primitives;
using Combat.Local.Application.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IMovementEffectOwnerRepository
    {
        void Create(MovementEffectOwner value);
        MovementEffectOwner Get(UnitId unitId);
        void Delete(UnitId target);
    }
}
