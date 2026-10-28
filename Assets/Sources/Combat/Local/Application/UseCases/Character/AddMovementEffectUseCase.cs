using Combat.Common;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using UnityEngine;

namespace Combat.Local.Application.UseCases
{
    public sealed class AddMovementEffectUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _repository;
        private readonly ICharacterMotionOutput _output;

        public AddMovementEffectUseCase(IPositionableRepository repository, ICharacterMotionOutput output, IUnitStore unitStore)
        {
            _repository = repository;
            _output = output;
            _unitStore = unitStore;
        }

        //TODO: DTO
        public void Execute(UnitId targetId, Vector3 direction, float speed, bool relative, float maxDuration)
        {
            var positionable = _repository.Get(targetId);

            Vector3 vector = direction.normalized * speed;

            MoveInDirectionEffect effect = new(new(vector.x, vector.y, vector.z), relative, maxDuration);
            positionable.ApplyForcedMovement(effect);
            var (Directional, Relative) = positionable.GetForcedVelocity();
            _output.Present(targetId, Directional, Relative, GetDesiredVelocity(_unitStore.Get(targetId), positionable));
        }

        private System.Numerics.Vector3 GetDesiredVelocity(UnitContext unit, Positionable positionable)
        {
            if (unit.Actor.CanMove == false || unit.LifeState.State == ConsciousState.Dead)
            {
                //UnityEngine.Vector3 velocity = positionable.Velocity;
                //positionable.Velocity = new UnityEngine.Vector3(0, velocity.y, 0);
                //_positionableRepository.Update(positionable);
                return System.Numerics.Vector3.Zero;
            }

            UnityEngine.Vector2 desiredDirection = new(unit.Actor.DesiredActions.MovementDirection.X, unit.Actor.DesiredActions.MovementDirection.Y);

            if (desiredDirection.sqrMagnitude > 1)
            {
                desiredDirection = desiredDirection.normalized;
            }

            float speed = positionable.Speed;
            var velocity = new UnityEngine.Vector3(desiredDirection.x, 0, desiredDirection.y) * speed;
            return new(velocity.x, velocity.y, velocity.z);
        }
    }
}
