using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class LifeStateEvaluateAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ICharacterDeathHandler _characterDeathHandler;
        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterLifeStateOutput;
        private readonly ISkillHitHandler _hitHandler;
        private readonly ISkillActionStateChangeHandler _stateChangeHandler;
        private readonly IActorActionUpdateOutput _actionUpdateOutput;

        public LifeStateEvaluateAllUseCase(IUnitStore unitStore,
            IHealthOutput healthOutput, ICharacterDeathHandler characterDeathHandler,
            ICharacterConsciousStateOutput characterConsciousStateOutput,
            ISkillHitHandler hitHandler, ISkillActionStateChangeHandler stateChangeHandler, IActorActionUpdateOutput actionUpdateOutput)
        {
            _unitStore = unitStore;
            _healthOutput = healthOutput;
            _characterLifeStateOutput = characterConsciousStateOutput;
            _characterDeathHandler = characterDeathHandler;
            _hitHandler = hitHandler;
            _stateChangeHandler = stateChangeHandler;
            _actionUpdateOutput = actionUpdateOutput;
        }

        public void Execute(ReadOnlySpan<Updatable> target, float deltaTime)
        {
            foreach (Updatable item in target)
            {
                ref UnitContext unit = ref _unitStore.Get(item.Id);

                switch (unit.LifeState.State)
                {
                    case ConsciousState.Alive:
                        UpdateAlive(ref unit);
                        continue;
                    case ConsciousState.Downed:
                        UpdateDowned(ref unit, deltaTime * item.TimeScale);
                        continue;
                    case ConsciousState.Dead:
                        continue;
                    default:
                        throw new InvalidOperationException($"Failed to update life state for {item.Id}: unknown state {unit.LifeState.State}.");
                }
            }
        }

        private void UpdateAlive(ref UnitContext unit)
        {
            if (unit.Health.Current > 0)
            {
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (unit.Composure.Current > 0 && damageSeverity <= LifeStateOwner.MaxDamageSeverity)
            {
                Down(ref unit);
                return;
            }

            Kill(ref unit);
        }

        private void UpdateDowned(ref UnitContext unit, float deltaTime)
        {
            if (unit.Health.Current > 0)
            {
                Revive(ref unit);
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (damageSeverity > LifeStateOwner.MaxDamageSeverity || unit.Composure.Current <= 0)
            {
                Kill(ref unit);
                return;
            }

            unit.Composure.TakeDamage(unit.Composure.Max * ComposureOwner.BaseDrainRate * deltaTime * damageSeverity);
            ComposureChangeRecord change = new(unit.Id, unit.Composure.Current, unit.Composure.Max);

            if (unit.Composure.Current <= 0)
            {
                Kill(ref unit);
            }

            _healthOutput.Present(change);
        }

        private void Down(ref UnitContext unit)
        {
            if (unit.LifeState.TryEnterDowned() == false)
            {
                return;
            }

            UnitId target = unit.Id;
            Interrupt(ref unit, InterruptReason.Downed);
            _characterLifeStateOutput.Present(target, ConsciousState.Downed);
        }

        private void Kill(ref UnitContext unit)
        {
            if (unit.LifeState.TryEnterDead() == false)
            {
                return;
            }

            UnitId target = unit.Id;
            Interrupt(ref unit, InterruptReason.Death);
            _characterLifeStateOutput.Present(target, ConsciousState.Dead);
            _characterDeathHandler.Handle(new KillRecord(target));
        }

        private void Revive(ref UnitContext unit)
        {
            if (unit.LifeState.TryRevive() == false)
            {
                return;
            }

            _characterLifeStateOutput.Present(unit.Id, ConsciousState.Alive);
        }

        private void Interrupt(ref UnitContext unit, InterruptReason reason)
        {
            if (unit.Actor.TryInterrupt(reason, out var interruptedAction) == false)
            {
                return;
            }

            if (interruptedAction.TryGet(out IAbilityAction abilityAction) == false)
            {
                return;
            }

            _actionUpdateOutput.Clear(unit.Id);
            AbilityKey key = new(unit.Id, abilityAction.Source);
            _hitHandler.Reset(key);
            _stateChangeHandler.Handle(key, ActionState.Inactive);
        }
    }
}
