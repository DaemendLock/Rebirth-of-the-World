using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface ISkillOwnerRepository
    {
        void Create(SkillOwner value);
        void Update(SkillOwner value);
        void Delete(UnitId id);
        bool TryGet(UnitId id, out SkillOwner skillOwner);
    }
}
