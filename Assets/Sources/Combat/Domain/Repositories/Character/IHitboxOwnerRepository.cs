using Combat.Common.Primitives;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.Repositories
{

    public interface IHitboxOwnerRepository
    {
        void Create(UnitId id);
        void Delete(UnitId id);
    }
}
