using Combat.Common.ValueObjects;
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

        private PlayerId _playerId;

        public PlayerController(DesireCastFromSlotUseCase castSkillFromSlotUseCase, ReleaseSkillFromSlotUseCase releaseSkillFromSlotUseCase, CreatePlayerUseCase createPlayerUseCase, DesireMoveInDirectionUseCase moveUseCase, RotateUseCase rotateUseCase, AssumeControlOverCharacterUseCase assumeControllOverCharacterUseCase)
        {
            _castSkillFromSlotUseCase = castSkillFromSlotUseCase;
            _releaseSkillFromSlotUseCase = releaseSkillFromSlotUseCase;
            _createPlayerUseCase = createPlayerUseCase;
            _moveUseCase = moveUseCase;
            _rotateUseCase = rotateUseCase;
            _assumeControllOverCharacterUseCase = assumeControllOverCharacterUseCase;

            _playerId = new(Guid.NewGuid());
            _createPlayerUseCase.Execute(_playerId);
        }

        public void TakeControll(UnitId id)
        {
            _assumeControllOverCharacterUseCase.Execute(_playerId, id);
        }

        public void MoveInDirection(Vector2 relativeDirection)
        {
            _moveUseCase.Execute(_playerId, new(relativeDirection.x, relativeDirection.y));
        }

        public void Rotate(Vector2 angle)
        {
            _rotateUseCase.Execute(_playerId, angle);
        }

        public void DesireCast(int slot)
        {
            _castSkillFromSlotUseCase.Execute(_playerId, slot);
        }

        public void ReleaseCast(int slot)
        {
            _releaseSkillFromSlotUseCase.Execute(_playerId, slot);
        }
    }
}
