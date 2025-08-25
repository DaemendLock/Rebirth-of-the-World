using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface ISkillRepository
    {
        void Create(Skill value);
        Skill Get(SkillId id);
        void Update(Skill value);
        void Delete(SkillId id);

        bool Contains(SkillId id);
    }
}
