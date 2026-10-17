using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.Repositories;

using UnityEngine;

namespace Combat.Local.Application.UseCases
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

        public void Execute(UnitId target, Vector2 angles)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (actor.CanMove == false)
            {
                return;
            }

            Positionable positionable = _positionableRepository.Get(target);
            positionable.Rotation *= Quaternion.Euler(0, angles.x, 0);
            _positionableRepository.Update(positionable);
        }
    }
}
