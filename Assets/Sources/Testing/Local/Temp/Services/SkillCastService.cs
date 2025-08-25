using Combat.Common.ValueObjects;
using Combat.Local.Domain.API;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.API.Skills;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Flags;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

namespace Testing.Local.Temp.Services
{
    public class SkillCastService : ISkillCastService
    {
        private readonly IActionRepository _actionRepository;
        private readonly ISkillRepository _skillRepository;

        private readonly SkillApiRepository _skillApiRepository;

        private readonly IAttributeEvaluationService _attributeEvaluationService;

        public SkillCastService(IActionRepository castActionRepository, ISkillRepository skillRepository, IAttributeEvaluationService attributeEvaluationService, SkillApiRepository skillApiRepository)
        {
            _actionRepository = castActionRepository;
            _skillRepository = skillRepository;
            _skillApiRepository = skillApiRepository;
            _attributeEvaluationService = attributeEvaluationService;
        }

        public void Cast(EntityId caster, SkillId skillId)
        {
            Skill skillData = _skillRepository.Get(skillId);

            ScriptedSkill skill = _skillApiRepository.Get(caster, skillId);

            if (skill.CanCast() == false)
            {
                return;
            }

            skill.OnCast();

            if (skill is ICastStateChangeHandler castHandler)
            {
                float hasteModifier = _attributeEvaluationService.GetHasteModifier(caster);
                IAction action = new CastAction(caster, hasteModifier, skillData.FrameData, skillData.Flags.HasFlag(SkillFlags.DontRestrictMovement), castHandler, null);
                _actionRepository.Create(action);

                action.Start();
            }
        }
    }
}
