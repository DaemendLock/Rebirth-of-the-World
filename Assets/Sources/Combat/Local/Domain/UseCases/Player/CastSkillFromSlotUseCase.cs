using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Repositories.Skills;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class CastSkillFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillActionsRepository _skillActionsRepository;
        private readonly IActionOutput _actionOutput;
        private readonly ActionFactory _actionFactory;

        public CastSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, ISkillRepository skillRepository, IActionOutput castOutput, ActionFactory actionFactory, ISkillActionsRepository skillActionsRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
            _skillActionsRepository = skillActionsRepository;
            _actionOutput = castOutput;
            _actionFactory = actionFactory;
        }

        public void Execute(EntityId caster, int slot)
        {
            if (TryGetSkillId(caster, slot, out SkillId skillId) == false)
            {
                Debug.Log("No skill in slot");
                return;
            }

            Skill skill = _skillRepository.Get(skillId, caster);

            if (skill.TryGetEffect(out SkillCastEffect effect) == false)
            {
                Debug.Log("Can't cast");
                return;
            }

            IReadOnlyCollection<ActionId> actions = _skillActionsRepository.Get(skillId);

            if (CanCast(caster, effect, actions) == false)
            {
                Debug.Log("Can't cast");
            }

            effect.Execute(caster);

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant))
            {
                return;
            }

            ActionId actionId = actions.First();
            StartCastAction(actionId, caster, skill, effect);
        }

        private bool CanCast(EntityId caster, SkillCastEffect effect, IReadOnlyCollection<ActionId> actions)
        {
            if (effect.CanCast(caster) != CastFailReason.Success)
            {
                return false;
            }

            if (actions == null || actions.Count == 0)
            {
                return true;
            }

            Actor actor = _actorRepository.Get(caster);

            return actor.CurrentAction == null;
        }

        private bool TryGetSkillId(EntityId owner, int slot, out SkillId result)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(owner);

            SkillId? skillId = skillOwner.GetSkill(slot);

            if (skillId.HasValue == false)
            {
                result = default;
                return false;
            }

            result = skillId.Value;
            return true;
        }

        private void StartCastAction(ActionId actionId, EntityId actorId, Skill source, SkillCastEffect castEffect)
        {
            Actor actor = _actorRepository.Get(actorId);

            actor.StartAction(_actionFactory.CreateCastAction(actionId, actorId, source.Id));
            _actorRepository.Update(actor);
            _actionOutput.Present(actorId, actionId);

            if (source.TryGetEffect(out SkillActionStateChangeEffect effect))
            {
                effect.Handle(ActionState.Startup);
            }
        }
    }

    public interface IActionOutput
    {
        void Present(EntityId id, ActionId skill);
    }
}
