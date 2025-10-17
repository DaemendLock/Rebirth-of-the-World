using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public interface IActionFactory
    {
        IAction CreateCastAction(Skill skill, EntityId actorId);
    }

    public class CastSkillFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillCastEventHandler _castSkillEventHandler;
        private readonly IActionStateChangeEventHandler _actionStateChangeEventHandler;
        private readonly ICastOutput _castOutput;
        private readonly IActionFactory _actionFactory;

        public CastSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, ISkillRepository skillRepository, ISkillCastEventHandler castSkillEventHandler, IActionStateChangeEventHandler actionStateChangeEventHandler, ICastOutput castOutput, IActionFactory actionFactory)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
            _castSkillEventHandler = castSkillEventHandler;
            _actionStateChangeEventHandler = actionStateChangeEventHandler;
            _castOutput = castOutput;
            _actionFactory = actionFactory;
        }

        public void Execute(EntityId caster, int slot)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(caster);

            SkillId? skillId = skillOwner.GetSkill(slot);

            if (skillId.HasValue == false)
            {
                Debug.Log("No skill in slot");
                return;
            }

            Skill skill = _skillRepository.Get(skillId.Value, caster);

            if (skill.CanCast == false)
            {
                return;
            }

            if (skill.StartAction == false)
            {
                _castSkillEventHandler.HandleEvent(caster, skillId.Value);
                return;
            }

            Actor actor = _actorRepository.Get(caster);

            actor.CurrentAction = _actionFactory.CreateCastAction(skill, caster);
            actor.CurrentAction.Start();
            _actorRepository.Update(actor);
            _castOutput.PlayAnimation(caster, skillId.Value);

            _castSkillEventHandler.HandleEvent(caster, skillId.Value);
            _actionStateChangeEventHandler.HandleEvent(caster, skillId.Value, ActionState.Startup);
        }
    }

    public interface ISkillCastEventHandler
    {
        void HandleEvent(EntityId? caster, SkillId skill);
    }

    public interface ICastOutput
    {
        void PlayAnimation(EntityId id, SkillId skill);
    }
}
