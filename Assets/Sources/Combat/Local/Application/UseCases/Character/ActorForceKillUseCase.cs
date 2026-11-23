using Combat.Common;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorForceKillUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ISkillHitHandler _hitHandler;
        private readonly ISkillActionStateChangeHandler _stateChangeHandler;
        private readonly ICharacterDeathHandler _deathHandler;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorForceKillUseCase(IUnitStore unitStore,
            ICharacterDeathHandler deathHandler, ICharacterConsciousStateOutput characterStateOutput,
            ISkillHitHandler hitHandler, ISkillActionStateChangeHandler stateChangeHandler)
        {
            _unitStore = unitStore;
            _hitHandler = hitHandler;
            _stateChangeHandler = stateChangeHandler;
            _deathHandler = deathHandler;
            _characterStateOutput = characterStateOutput;
        }

        public void Execute(UnitId target, AbilityKey? source)
        {
            UnitContext unit = _unitStore.Get(target);

            if (unit.LifeState.TryEnterDead() == false)
            {
                return;
            }

            Interrupt(unit);
            _characterStateOutput.Present(target, ConsciousState.Dead);
            _deathHandler.Handle(new KillRecord(target));
        }

        private void Interrupt(UnitContext unit)
        {
            CastActivity cast = unit.Actor.CastActivity;
            unit.Actor.TryInterrupt(InterruptReason.Death, out ActivityState interruptedAction);

            if (interruptedAction.Type == ActivityType.Casting)
            {
                AbilityKey key = new(unit.Id, cast.Skill);
                _hitHandler.Reset(key);
                _stateChangeHandler.Handle(key, ActionState.Inactive);
            }
        }
    }
}
