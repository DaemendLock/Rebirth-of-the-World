using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Infrastructure.Factories
{
    public interface ISkillFactory
    {
        public Skill Create(SkillId id);
    }
}
