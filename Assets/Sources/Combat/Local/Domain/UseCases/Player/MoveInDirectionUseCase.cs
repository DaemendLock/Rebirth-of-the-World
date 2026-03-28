using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class MoveInDirectionUseCase
    {
        private readonly IPositionableRepository _positionableRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStateRepository _stateRepository;

        private readonly IMovementOutput _movementOutput;

        public MoveInDirectionUseCase(IPositionableRepository positionableRepository, IAttributesRepository attributesRepository, IActorRepository actorRepository, IMovementOutput movementOutput, IStateRepository stateRepository)
        {
            _positionableRepository = positionableRepository;
            _attributesRepository = attributesRepository;
            _actorRepository = actorRepository;
            _movementOutput = movementOutput;
            _stateRepository = stateRepository;
        }

        public void Execute(EntityId id, Vector2 relativeDirection)
        {
            if (_stateRepository.Get(id).ConsciousState == Entities.ConsciousState.Dead)
            {
                return;
            }

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
