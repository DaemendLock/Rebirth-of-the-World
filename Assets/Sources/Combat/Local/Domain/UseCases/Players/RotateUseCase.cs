using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class RotateUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPositionableRepository _positionableRepository;

        public RotateUseCase(IActorRepository actorRepository, IPositionableRepository positionableRepository, IPlayerRepository playerRepository)
        {
            _actorRepository = actorRepository;
            _positionableRepository = positionableRepository;
            _playerRepository = playerRepository;
        }

        public void Execute(PlayerId playerId, Vector2 angles)
        {
            Player player = _playerRepository.Get(playerId);

            if (player.ControlledEntity.HasValue == false)
            {
                return;
            }

            UnitId entityId = player.ControlledEntity.Value;

            if (_actorRepository.TryGet(entityId, out Actor actor) == false)
            {
                return;
            }

            if (actor.CanMove == false)
            {
                return;
            }

            Positionable positionable = _positionableRepository.Get(entityId);
            positionable.Rotation *= Quaternion.Euler(0, angles.x, 0);
            _positionableRepository.Update(positionable);
        }
    }
}
