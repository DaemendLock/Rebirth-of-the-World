using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Services;
using Combat.Local.Domain.UseCases;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public sealed class PlayerController
    {
        private readonly ActorReleaseSkillFromSlotUseCase _releaseSkillFromSlotUseCase;
        private readonly DesireMoveInDirectionUseCase _moveUseCase;
        private readonly ActorDesireCastFromSlotUseCase _desireCastFromSlotUseCase;
        private readonly RotateUseCase _rotateUseCase;
        private readonly AssumeControlOverCharacterUseCase _assumeControllOverCharacterUseCase;
        private readonly IEncounterStateMachine _encounterState;
        private readonly PlayerSession _playerSession;

        public PlayerController(ActorReleaseSkillFromSlotUseCase releaseSkillFromSlotUseCase, DesireMoveInDirectionUseCase moveUseCase,
                                RotateUseCase rotateUseCase, AssumeControlOverCharacterUseCase assumeControllOverCharacterUseCase,
                                IEncounterStateMachine encounterState, PlayerSession playerSession, ActorDesireCastFromSlotUseCase desireCastFromSlotUseCase)
        {
            _releaseSkillFromSlotUseCase = releaseSkillFromSlotUseCase;
            _moveUseCase = moveUseCase;
            _rotateUseCase = rotateUseCase;
            _assumeControllOverCharacterUseCase = assumeControllOverCharacterUseCase;
            _encounterState = encounterState;
            _playerSession = playerSession;
            _desireCastFromSlotUseCase = desireCastFromSlotUseCase;
        }

        public bool AcceptsInput => _encounterState.IsRunning;

        public void TakeControll(UnitId id)
        {
            if (_encounterState.State != EncounterState.Starting && AcceptsInput == false) return;

            _assumeControllOverCharacterUseCase.Execute(id);
        }

        public void MoveInDirection(Vector2 relativeDirection)
        {
            if (AcceptsInput == false) return;

            if (_playerSession.ControlledUnitId.HasValue == false)
            {
                return;
            }

            _moveUseCase.Execute(_playerSession.ControlledUnitId.Value, new(relativeDirection.x, relativeDirection.y));
        }

        public void Rotate(Vector2 angle)
        {
            if (AcceptsInput == false) return;

            if (_playerSession.ControlledUnitId.HasValue == false)
            {
                return;
            }

            _rotateUseCase.Execute(_playerSession.ControlledUnitId.Value, angle);
        }

        public void DesireCast(int slot)
        {
            if (AcceptsInput == false) return;

            if (_playerSession.ControlledUnitId.HasValue == false)
            {
                return;
            }

            _desireCastFromSlotUseCase.Execute(_playerSession.ControlledUnitId.Value, slot);
        }

        public void ReleaseCast(int slot)
        {
            if (AcceptsInput == false) return;

            if (_playerSession.ControlledUnitId.HasValue == false)
            {
                return;
            }

            _releaseSkillFromSlotUseCase.Execute(_playerSession.ControlledUnitId.Value, slot);
        }
    }
}
