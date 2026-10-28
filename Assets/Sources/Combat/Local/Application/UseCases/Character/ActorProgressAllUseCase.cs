using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorProgressAllUseCase
    {
        private readonly ISkillActionStateChangeHandler _skillActionStateChangeHandler;
        private readonly ISkillHitHandler _skillHitHandler;

        private readonly IActorActionUpdateOutput _actorUpdateOutput;

        public ActorProgressAllUseCase(IActorActionUpdateOutput actionUpdateOutput, ISkillActionStateChangeHandler skillActionStateChangeHandler, ISkillHitHandler skillHitHandler)
        {
            _skillActionStateChangeHandler = skillActionStateChangeHandler;
            _skillHitHandler = skillHitHandler;
            _actorUpdateOutput = actionUpdateOutput;
        }

        public void Execute(System.ReadOnlySpan<UnitContext> targets, float deltaTime)
        {
            foreach (UnitContext target in targets)
            {
                try
                {
                    HandleTarget(target, deltaTime * target.Attributes.TimeScale);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        private void HandleTarget(UnitContext unit, float deltaTime)
        {
            Action action = unit.Actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            if (unit.LifeState.State != ConsciousState.Alive)
            {
                InterruptReason reason = unit.LifeState.State == ConsciousState.Dead ? InterruptReason.Death : InterruptReason.Downed;

                if (unit.Actor.TryInterrupt(reason))
                {
                    _actorUpdateOutput.Clear(unit.Id);

                    if (action.TryGet(out IAbilityAction abilityAction))
                    {
                        AbilityKey key = new(unit.Id, abilityAction.Source);
                        _skillHitHandler.Reset(key);
                        _skillActionStateChangeHandler.Handle(key, ActionState.Inactive);
                        return;
                    }
                }

                return;
            }

            if (action.IsComplete)
            {
                StopAction(unit, action);
                return;
            }

            bool isAbilityAction = action.TryGet(out IAbilityAction abilityActionStrategy);
            ActionState oldState = isAbilityAction ? abilityActionStrategy.State : default;

            action.Progress(deltaTime);

            if (action.Flags.HasFlag(ActionFlags.Holdable) && abilityActionStrategy.State == ActionState.Recovery)
            {
                _actorUpdateOutput.Release(unit.Id);
            }

            if (isAbilityAction && abilityActionStrategy.State != oldState)
            {
                HandleStateChanges(unit.Id, abilityActionStrategy, oldState);
                return;
            }
        }

        private void StopAction(UnitContext unit, Action action)
        {
            unit.Actor.StopAction();
            _actorUpdateOutput.Clear(unit.Id);

            if (action.TryGet(out IAbilityAction abilityAction))
            {
                AbilityKey key = new(unit.Id, abilityAction.Source);
                _skillHitHandler.Reset(key);
                _skillActionStateChangeHandler.Handle(key, ActionState.Inactive);
            }
        }

        private void HandleStateChanges(UnitId actorId, IAbilityAction action, ActionState oldState)
        {
            if (action.State == oldState)
            {
                return;
            }

            AbilityKey key = new(actorId, action.Source);
            _skillHitHandler.Reset(key);
            _skillActionStateChangeHandler.Handle(key, action.State);
        }
    }
}
