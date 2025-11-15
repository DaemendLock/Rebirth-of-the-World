using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using System.Linq;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
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
                Debug.Log("Can't cast");
                return;
            }

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant))
            {
                _castSkillEventHandler.HandleEvent(caster, skillId.Value);
                return;
            }

            if (skill.AssociatedActions.Count == 0)
            {
                return;
            }

            ActionId actionId = skill.AssociatedActions.First();
            StartCastAction(actionId, caster, skillId.Value);
        }

        private void StartCastAction(ActionId actionId, EntityId actorId, SkillId source)
        {
            Actor actor = _actorRepository.Get(actorId);

            if (actor.CurrentAction != null)
            {
                return;
            }

            actor.StartAction(_actionFactory.CreateCastAction(actionId, actorId));
            _actorRepository.Update(actor);

            _castSkillEventHandler.HandleEvent(actorId, source);
            _castOutput.Present(actorId, actionId);
            _actionStateChangeEventHandler.HandleEvent(actorId, ActionState.Startup);
        }
    }

    public interface ISkillCastEventHandler
    {
        void HandleEvent(EntityId? caster, SkillId skill);
    }

    public interface ICastOutput
    {
        void Present(EntityId id, ActionId skill);
    }
}
