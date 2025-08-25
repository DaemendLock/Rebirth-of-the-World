using Server.Combat.Data.Entities;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Data.Repositories
{
    public interface ISkillDataRepository
    {
        void Add(SkillData skill);
        SkillData Get(SkillId id);
        void Remove(SkillId id);
    }
}
