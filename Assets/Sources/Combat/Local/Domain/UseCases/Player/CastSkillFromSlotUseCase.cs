using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Factories;
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
        private readonly IActionOutput _actionOutput;
        private readonly ActionFactory _actionFactory;

        public CastSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, ISkillRepository skillRepository, IActionOutput castOutput, ActionFactory actionFactory)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
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

            if (CanCast(skill, effect) == false)
            {
                Debug.Log("Can't cast");
            }

            effect.Execute(caster);

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant))
            {
                return;
            }

            ActionId actionId = skill.Actions.First();
            StartCastAction(actionId, caster, skill);
        }

        private bool CanCast(Skill skill, SkillCastEffect effect)
        {
            if (effect.CanCast(skill.Owner) != CastFailReason.Success)
            {
                return false;
            }

            if (skill.Flags.HasFlag(Common.Flags.SkillFlags.Instant))
            {
                return true;
            }

            if (skill.Owner == null)
            {
                return false;
            }

            Actor actor = _actorRepository.Get(skill.Owner.Value);

            return actor.CurrentAction == null;
        }

        private void StartCastAction(ActionId actionId, EntityId actorId, Skill source)
        {
            Actor actor = _actorRepository.Get(actorId);

            actor.CurrentAction = _actionFactory.CreateCastAction(actionId, actorId, source.Id);
            actor.CurrentAction.Start();
            _actorRepository.Update(actor);
            _actionOutput.Present(actor);

            if (source.TryGetEffect(out SkillActionStateChangeEffect effect))
            {
                effect.Handle(ActionState.Startup);
            }
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
    }

    public interface IActionOutput
    {
        void Present(Actor actor);
    }
}
