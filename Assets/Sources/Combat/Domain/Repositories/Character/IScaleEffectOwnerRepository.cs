using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IScaleEffectOwnerRepository
    {
        void Create(ScaleEffectOwner value);
        ScaleEffectOwner Get(UnitId target);
        void Delete(UnitId target);
    }
}
