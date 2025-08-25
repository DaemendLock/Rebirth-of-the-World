using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Infrastructure.Factories
{
    public interface ISkillScriptFactory
    {
        ISkillScript Create(Skill skill, Unit caster);
    }
}
