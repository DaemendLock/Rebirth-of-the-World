using Combat.Common.Primitives;
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
        private readonly PlayerDesireCastFromSlotUseCase _castSkillFromSlotUseCase;
        private readonly PlayerReleaseSkillFromSlotUseCase _releaseSkillFromSlotUseCase;
        private readonly CreatePlayerUseCase _createPlayerUseCase;
        private readonly DesireMoveInDirectionUseCase _moveUseCase;
        private readonly RotateUseCase _rotateUseCase;
        private readonly AssumeControlOverCharacterUseCase _assumeControllOverCharacterUseCase;
        private readonly IEncounterStateMachine _encounterState;

        public PlayerController(PlayerDesireCastFromSlotUseCase castSkillFromSlotUseCase,
                                PlayerReleaseSkillFromSlotUseCase releaseSkillFromSlotUseCase,
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
        }

        public bool AcceptsInput => _encounterState.IsRunning;

        public PlayerId Create()
        {
            return _createPlayerUseCase.Execute();
        }

        public void TakeControll(PlayerId playerId, UnitId id)
        {
            if (_encounterState.State != EncounterState.Starting && AcceptsInput == false) return;

            _assumeControllOverCharacterUseCase.Execute(playerId, id);
        }

        public void MoveInDirection(PlayerId playerId, Vector2 relativeDirection)
        {
            if (AcceptsInput == false) return;

            _moveUseCase.Execute(playerId, new(relativeDirection.x, relativeDirection.y));
        }

        public void Rotate(PlayerId playerId, Vector2 angle)
        {
            if (AcceptsInput == false) return;

            _rotateUseCase.Execute(playerId, angle);
        }

        public void DesireCast(PlayerId playerId, int slot)
        {
            if (AcceptsInput == false) return;

            _castSkillFromSlotUseCase.Execute(playerId, slot);
        }

        public void ReleaseCast(PlayerId playerId, int slot)
        {
            if (AcceptsInput == false) return;

            _releaseSkillFromSlotUseCase.Execute(playerId, slot);
        }
    }
}
