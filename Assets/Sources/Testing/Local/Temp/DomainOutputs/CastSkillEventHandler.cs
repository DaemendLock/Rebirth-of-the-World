using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class CastSkillEventHandler : ICastSkillEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly StartActionUseCase _startActionUseCase;

        public CastSkillEventHandler(SkillApiProvider skillApiRepository, StartActionUseCase startActionUseCase)
        {
            _skillApiProvider = skillApiRepository;
            _startActionUseCase = startActionUseCase;
        }

        public CastFailReason HandleEvent(EntityId? casterId, SkillId skillId)
        {
            SkillApi skillInfo = _skillApiProvider.Get(skillId, casterId);

            if (skillInfo == null)
            {
                return CastFailReason.UnknownSkill;
            }

            if (skillInfo.TryGetProperty(out ICastableSkill castable) == false)
            {
                return CastFailReason.NotCastable;
            }

            castable.OnCast();

            if (casterId.HasValue && castable is ICastStateChangeHandler actionHandler)
            {
                _startActionUseCase.Execute(casterId.Value, skillId);
            }

            return CastFailReason.Success;
        }
    }
}
