using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface ISkillOwnerRepository
    {
        void Create(SkillOwner value);
        void Delete(UnitId id);
        bool TryGet(UnitId id, out SkillOwner skillOwner);
    }
}
