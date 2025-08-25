using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Services
{
    public class MovementService : IMovementService
    {
        private readonly IActionRepository _actionRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IAttributeEvaluationService _attributeEvaluationService;

        public MovementService(IActionRepository actionRepository, IPositionRepository positionRepository, IAttributeEvaluationService attributeEvaluationService)
        {
            _actionRepository = actionRepository;
            _positionRepository = positionRepository;
            _attributeEvaluationService = attributeEvaluationService;
        }

        public bool CanMove(EntityId id)
        {
            IAction currectAction = _actionRepository.Get(id);

            if (currectAction == null)
            {
                return true;
            }

            if (currectAction.IsActive == false)
            {
                return true;
            }

            return currectAction.AllowMovement;
        }

        public void SetPosition(EntityId id, Vector3 position)
        {
            var value = _positionRepository.Get(id);
            value.Position = position;
            _positionRepository.Update(value);
        }

        public bool TryMoveInDirection(EntityId id, Vector2 relativeDirection, out Vector3 velocity)
        {
            if (CanMove(id) == false)
            {
                velocity = default;
                return false;
            }

            if (relativeDirection.sqrMagnitude > 1)
            {
                relativeDirection = relativeDirection.normalized;
            }

            float speed = _attributeEvaluationService.GetAttributeValue(id, Attribute.Speed);

            velocity = Quaternion.Euler(0, _positionRepository.Get(id).Rotation, 0) * new Vector3(relativeDirection.x, 0, relativeDirection.y) * speed;
            return true;
        }
    }

    public interface IMovementService
    {
        void SetPosition(EntityId id, Vector3 position);
        bool CanMove(EntityId target);
        bool TryMoveInDirection(EntityId target, Vector2 relativeDirection, out Vector3 speed);
    }
}
