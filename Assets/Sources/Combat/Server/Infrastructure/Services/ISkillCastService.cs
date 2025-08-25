using Server.Combat.Domain.Entities;

namespace Server.Combat.Infrastructure.Services
{
    public interface ISkillCastService
    {
        bool CanCast(Skill skill, Unit caster);

        void Cast(Skill skill, Unit caster);
    }
}
