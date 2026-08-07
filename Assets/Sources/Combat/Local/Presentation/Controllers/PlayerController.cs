using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Players;

using System;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public sealed class PlayerController : MonoBehaviour
    {
        [Zenject.Inject] private readonly DesireCastFromSlotUseCase _castSkillFromSlotUseCase;
        [Zenject.Inject] private readonly ReleaseSkillFromSlotUseCase _releaseSkillFromSlotUseCase;
        [Zenject.Inject] private readonly CreatePlayerUseCase _createPlayerUseCase;
        [Zenject.Inject] private readonly DesireMoveInDirectionUseCase _moveUseCase;
        [Zenject.Inject] private readonly RotateUseCase _rotateUseCase;
        [Zenject.Inject] private readonly AssumeControlOverCharacterUseCase _assumeControllOverCharacterUseCase;

        private PlayerId _playerId;

        private void Start()
        {
            _playerId = new(Guid.NewGuid());
            _createPlayerUseCase.Execute(_playerId);
        }

        private void OnDestroy()
        {
            //
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
