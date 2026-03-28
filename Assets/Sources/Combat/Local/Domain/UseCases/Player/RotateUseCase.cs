using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class RotateUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly IPositionableRepository _positionableRepository;

        public RotateUseCase(IActorRepository actorRepository, IPositionableRepository positionableRepository)
        {
            _actorRepository = actorRepository;
            _positionableRepository = positionableRepository;
        }

        public void Execute(EntityId entityId, Vector2 angles)
        {
            UnityEngine.Debug.Log(angles);

            Actor actor = _actorRepository.Get(entityId);

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
