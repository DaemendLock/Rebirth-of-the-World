using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Players;

using System;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public sealed class PlayerController
    {
        private readonly DesireCastFromSlotUseCase _castSkillFromSlotUseCase;
        private readonly ReleaseSkillFromSlotUseCase _releaseSkillFromSlotUseCase;
        private readonly CreatePlayerUseCase _createPlayerUseCase;
        private readonly DesireMoveInDirectionUseCase _moveUseCase;
        private readonly RotateUseCase _rotateUseCase;
        private readonly AssumeControlOverCharacterUseCase _assumeControllOverCharacterUseCase;
        private readonly IEncounterStateMachine _encounterState;

        private PlayerId _playerId;

        public PlayerController(DesireCastFromSlotUseCase castSkillFromSlotUseCase,
                                ReleaseSkillFromSlotUseCase releaseSkillFromSlotUseCase,
                                CreatePlayerUseCase createPlayerUseCase,
                                DesireMoveInDirectionUseCase moveUseCase,
                                RotateUseCase rotateUseCase,
                                AssumeControlOverCharacterUseCase assumeControllOverCharacterUseCase,
                                IEncounterStateMachine encounterState)
        {
            _castSkillFromSlotUseCase = castSkillFromSlotUseCase;
            _releaseSkillFromSlotUseCase = releaseSkillFromSlotUseCase;
            _createPlayerUseCase = createPlayerUseCase;
            _moveUseCase = moveUseCase;
            _rotateUseCase = rotateUseCase;
            _assumeControllOverCharacterUseCase = assumeControllOverCharacterUseCase;
            _encounterState = encounterState;

            _playerId = new(Guid.NewGuid());
            _createPlayerUseCase.Execute(_playerId);
        }

        public bool AcceptsInput => _encounterState.IsRunning;

        public void TakeControll(UnitId id)
        {
            if (_encounterState.State != EncounterState.Starting && AcceptsInput == false) return;

            _assumeControllOverCharacterUseCase.Execute(_playerId, id);
        }

        public void MoveInDirection(Vector2 relativeDirection)
        {
            if (AcceptsInput == false) return;

            _moveUseCase.Execute(_playerId, new(relativeDirection.x, relativeDirection.y));
        }

        public void Rotate(Vector2 angle)
        {
            if (AcceptsInput == false) return;

            _rotateUseCase.Execute(_playerId, angle);
        }

        public void DesireCast(int slot)
        {
            if (AcceptsInput == false) return;

            _castSkillFromSlotUseCase.Execute(_playerId, slot);
        }

        public void ReleaseCast(int slot)
        {
            if (AcceptsInput == false) return;

            _releaseSkillFromSlotUseCase.Execute(_playerId, slot);
        }
    }
}
