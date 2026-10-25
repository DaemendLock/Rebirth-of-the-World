using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.Entities;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorReleaseSkillFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorActionUpdateOutput _actionUpdateOutput;

        public ActorReleaseSkillFromSlotUseCase(IUnitStore units, IActorActionUpdateOutput actionUpdateOutput)
        {
            _unitStore = units;
            _actionUpdateOutput = actionUpdateOutput;
        }

        public void Execute(UnitId caster, int slot)
        {
            UnitContext unit = _unitStore.Get(caster);

            Action action = unit.Actor.CurrentAction;

            if (action == null || (action.TryGet(out IAbilityAction abilityAction) == false))
            {
                return;
            }

            SkillId? skillId = unit.Skills.GetSkill(slot);

            if ((skillId.HasValue == false) || abilityAction.Source != skillId.Value)
            {
                return;
            }

            if (action.IsHoldable && action.TryGet(out IReleasableAction releasable))
            {
                releasable.Release();
            }

            if (action.IsHoldable && abilityAction.State == Common.ValueObjects.ActionState.Recovery)
            {
                _actionUpdateOutput.Release(caster);
            }
        }
    }
}
