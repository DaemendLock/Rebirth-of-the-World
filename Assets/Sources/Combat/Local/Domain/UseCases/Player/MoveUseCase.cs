using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class MoveUseCase
    {
        private readonly IPositionableRepository _positionableRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IActorRepository _actorRepository;

        private readonly IMovementOutput _movementOutput;

        public MoveUseCase(IPositionableRepository positionableRepository, IAttributesRepository attributesRepository, IActorRepository actorRepository, IMovementOutput movementOutput)
        {
            _positionableRepository = positionableRepository;
            _attributesRepository = attributesRepository;
            _actorRepository = actorRepository;
            _movementOutput = movementOutput;
        }

        public void Execute(EntityId id, Vector2 relativeDirection)
        {
            if (_actorRepository.Get(id).CanMove == false)
            {
                return;
            }

            if (relativeDirection.sqrMagnitude > 1)
            {
                relativeDirection = relativeDirection.normalized;
            }

            float speed = _attributesRepository.Get(id).GetAttributeValue(Attribute.Speed);

            Vector3 velocity = _positionableRepository.Get(id).Rotation * new Vector3(relativeDirection.x, 0, relativeDirection.y) * speed;
            _movementOutput.SetHorizontalVelocity(id, velocity);
        }
    }

    public interface IMovementOutput
    {
        void SetHorizontalVelocity(EntityId target, Vector3 velocity);
    }
}
