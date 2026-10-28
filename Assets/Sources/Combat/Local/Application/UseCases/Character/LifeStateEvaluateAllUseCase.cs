using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.OutputPorts;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class LifeStateEvaluateAllUseCase
    {
        private readonly ICharacterDeathHandler _characterDeathHandler;
        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterLifeStateOutput;
        private readonly ISkillHitHandler _hitHandler;
        private readonly ISkillActionStateChangeHandler _stateChangeHandler;
        private readonly IActorActionUpdateOutput _actionUpdateOutput;

        public LifeStateEvaluateAllUseCase(IHealthOutput healthOutput, ICharacterDeathHandler characterDeathHandler,
            ICharacterConsciousStateOutput characterConsciousStateOutput,
            ISkillHitHandler hitHandler, ISkillActionStateChangeHandler stateChangeHandler, IActorActionUpdateOutput actionUpdateOutput)
        {
            _healthOutput = healthOutput;
            _characterLifeStateOutput = characterConsciousStateOutput;
            _characterDeathHandler = characterDeathHandler;
            _hitHandler = hitHandler;
            _stateChangeHandler = stateChangeHandler;
            _actionUpdateOutput = actionUpdateOutput;
        }

        public void Execute(ReadOnlySpan<UnitContext> target)
        {
            foreach (UnitContext unit in target)
            {
                switch (unit.LifeState.State)
                {
                    case ConsciousState.Alive:
                        UpdateAlive(unit);
                        continue;
                    case ConsciousState.Downed:
                        UpdateDowned(unit);
                        continue;
                    case ConsciousState.Dead:
                        continue;
                    default:
                        throw new InvalidOperationException($"Failed to update life state for {unit.Id}: unknown state {unit.LifeState.State}.");
                }
            }
        }

        private void UpdateAlive(UnitContext unit)
        {
            if (unit.Health.Current > 0)
            {
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (unit.Composure.Current > 0 && damageSeverity <= LifeStateOwner.MaxDamageSeverity)
            {
                Down(unit);
                return;
            }

            Kill(unit);
        }

        private void UpdateDowned(UnitContext unit)
        {
            if (unit.Health.Current > 0)
            {
                Revive(unit);
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (damageSeverity > LifeStateOwner.MaxDamageSeverity || unit.Composure.Current <= 0)
            {
                Kill(unit);
            }
        }

        private void Down(UnitContext unit)
        {
            if (unit.LifeState.TryEnterDowned() == false)
            {
                return;
            }

            UnitId target = unit.Id;
            Interrupt(unit, InterruptReason.Downed);
            _characterLifeStateOutput.Present(target, ConsciousState.Downed);
        }

        private void Kill(UnitContext unit)
        {
            if (unit.LifeState.TryEnterDead() == false)
            {
                return;
            }

            UnitId target = unit.Id;
            Interrupt(unit, InterruptReason.Death);
            _characterLifeStateOutput.Present(target, ConsciousState.Dead);
            _characterDeathHandler.Handle(new KillRecord(target));
        }

        private void Revive(UnitContext unit)
        {
            if (unit.LifeState.TryRevive() == false)
            {
                return;
            }

            _characterLifeStateOutput.Present(unit.Id, ConsciousState.Alive);
        }

        private void Interrupt(UnitContext unit, InterruptReason reason)
        {
            if (unit.Actor.TryInterrupt(reason, out var interruptedAction) == false)
            {
                return;
            }

            _actionUpdateOutput.Clear(unit.Id);

            if (interruptedAction.TryGet(out IAbilityAction abilityAction) == false)
            {
                return;
            }

            AbilityKey key = new(unit.Id, abilityAction.Source);
            _hitHandler.Reset(key);
            _stateChangeHandler.Handle(key, ActionState.Inactive);
        }
    }
}
