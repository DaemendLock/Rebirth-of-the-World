using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;

using System.Linq;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class ActorCastSkillFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IAbilityFactory _abilityFactory;
        private readonly ActionFactory _actionFactory;
        private readonly IActorActionUpdateOutput _actorUpdateOutput;
        private readonly ISkillHitHandler _skillHitHandler;
        private readonly ISkillExecutionPort _skillExecutionPort;
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;

        public ActorCastSkillFromSlotUseCase(IUnitStore unitStore, IAbilityFactory abilityFactory, ActionFactory actionFactory, IActorActionUpdateOutput actorUpdateOutput, ISkillHitHandler skillHitHandler, ISkillExecutionPort skillExecutionPort, ISkillActionStateChangeHandler skillActionStateChangeHandler)
        {
            _unitStore = unitStore;
            _abilityFactory = abilityFactory;
            _actionFactory = actionFactory;
            _actorUpdateOutput = actorUpdateOutput;
            _skillHitHandler = skillHitHandler;
            _skillExecutionPort = skillExecutionPort;
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
        }

        public void Execute(UnitId target, int skillSlot)
        {
            if (_unitStore.TryGet(target, out UnitContext unit) == false)
            {
                return;
            }

            var skill = unit.Skills.GetSkill(skillSlot);

            if (skill.HasValue == false)
            {
                return;
            }

            Cast(unit, skill.Value);
        }

        private bool Cast(UnitContext unit, SkillId skillId)
        {
            UnitId caster = unit.Id;

            if (unit.LifeState.State != ConsciousState.Alive)
            {
                return true;
            }

            if (unit.Skills.GetCooldown(skillId) > 0)
            {
                return true;
            }

            AbilityKey abilityKey = new(caster, skillId);
            SkillInfo ability = _abilityFactory.Create(skillId);

            if (CanCast(unit, ability.Flags, abilityKey) != CastFailReason.Success)
            {
                UnityEngine.Debug.Log("Can't cast - cast forbidden");
                return true;
            }

            bool beginAction = _skillExecutionPort.BeginCast(abilityKey);

            if (ability.BaseCooldown > 0)
            {
                unit.Skills.SetCooldown(skillId, ability.BaseCooldown);
            }

            if (beginAction == false || unit.LifeState.State != ConsciousState.Alive)
            {
                return true;
            }

            if (ability.Actions != null && ability.Actions.Count > 0)
            {
                return StartCastAction(unit, ability.Actions.First(), skillId, ability.Flags);
            }

            return true;
        }

        private CastFailReason CanCast(UnitContext unit, SkillFlags skillFlags, AbilityKey abilityKey)
        {
            if (unit.LifeState.State != ConsciousState.Alive)
            {
                return CastFailReason.CantCast;
            }

            Action action = unit.Actor.CurrentAction;

            if (skillFlags.HasFlag(SkillFlags.Instant) || action == null)
            {
                return _skillExecutionPort.CanCast(abilityKey);
            }

            if (action.TryGet(out IAbilityAction abilityAction) == false)
            {
                return CastFailReason.CastInProgress;
            }

            if (abilityAction.State != ActionState.Recovery)
            {
                if (abilityAction.State != ActionState.Inactive)
                {
                    return CastFailReason.CastInProgress;
                }
            }

            if (action.TryGet(out IChainableAction chainableAction) == false)
            {
                return CastFailReason.CastInProgress;
            }

            if (chainableAction.CanChainInto(abilityKey.Skill) == false)
            {
                return CastFailReason.CastInProgress;
            }

            return _skillExecutionPort.CanCast(abilityKey);
        }

        private bool StartCastAction(UnitContext unit, ActionId actionId, SkillId skillId, SkillFlags skillFlags)
        {
            if (unit.Actor.TryInterrupt(InterruptReason.Chained, out Action oldAction))
            {
                _actorUpdateOutput.Clear(unit.Id);

                if (oldAction.TryGet(out IAbilityAction oldAbilityAction))
                {
                    AbilityKey oldKey = new(unit.Id, oldAbilityAction.Source);
                    _skillHitHandler.Reset(oldKey);
                    _skillActionStateChangeHandler.Handle(oldKey, ActionState.Inactive);

                    if (unit.LifeState.State != ConsciousState.Alive)
                    {
                        return true;
                    }
                }
            }

            Action newAction = _actionFactory.CreateCastAction(actionId, skillId, skillFlags);
            unit.Actor.StartAction(newAction);
            _actorUpdateOutput.Present(unit.Id, newAction.Id, newAction.Flags, newAction.ActiveTime);
            _skillActionStateChangeHandler.Handle(new(unit.Id, skillId), ActionState.Startup);
            return true;
        }
    }
}
