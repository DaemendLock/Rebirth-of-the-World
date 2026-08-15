using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IScaleEffectOwnerRepository
    {
        void Create(ScaleEffectOwner value);
        ScaleEffectOwner Get(UnitId target);
        void Update(ScaleEffectOwner value);
        void Delete(UnitId target);
    }
}
