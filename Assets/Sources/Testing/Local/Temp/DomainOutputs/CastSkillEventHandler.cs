using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class StartActionUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IFrameDataRepository _frameDataRepository;
        private readonly IActionStateChangeEventHandler _actionStateChangeEventHandler;

        public StartActionUseCase(IActorRepository actorRepository, IAttributesRepository attributesRepository, IFrameDataRepository frameDataRepository, IActionStateChangeEventHandler actionStateChangeEventHandler)
        {
            _actorRepository = actorRepository;
            _attributesRepository = attributesRepository;
            _frameDataRepository = frameDataRepository;
            _actionStateChangeEventHandler = actionStateChangeEventHandler;
        }

        public void Execute(EntityId actorId, SkillId skillId)
        {
            Actor actor = _actorRepository.Get(actorId);
            Attributes attributes = _attributesRepository.Get(actorId);

            actor.CurrentAction = new CastAction(new(skillId, true, 0, 0, false), attributes.GetHasteModifier(), _frameDataRepository.Get(skillId).FrameData);
            actor.CurrentAction.Start();
            _actorRepository.Update(actor);
            _actionStateChangeEventHandler.HandleEvent(actorId, skillId, ActionState.Startup);
        }
    }

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
