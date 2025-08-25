using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Repositories
{
    public interface ISkillRepository
    {
        void Add(Skill skill);
        Skill Get(SkillId id);
        void Remove(SkillId id);
    }
}
