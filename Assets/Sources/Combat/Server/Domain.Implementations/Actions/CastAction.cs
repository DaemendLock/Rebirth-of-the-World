using Server.Combat.Domain.Entities;
using Server.Combat.Infrastructure.Services;

namespace Server.Combat.Domain.Implementations.Actions
{
    public class CastAction // : IAction
    {
        private readonly int _skillIndex;
        private readonly ISkillCastService _skillCastService;

        public CastAction(int skillIndex, ISkillCastService skillCastService)
        {
            _skillIndex = skillIndex;
            _skillCastService = skillCastService;
        }

        public bool CanPerformBy(Unit actor)
        {
            //Skill skill = actor.GetSkillByIndex(_skillIndex);

            //if (skill == null)
            //    return false;

            return true; //_skillCastService.CanCast(skill, actor);
        }

        public void PerformBy(Unit caster)
        {
            //ISkill skill = caster.GetSkillByIndex(_skillIndex);

            if (caster == null)
                return;

            //_skillCastService.Cast(skill, caster);
        }
    }
}
