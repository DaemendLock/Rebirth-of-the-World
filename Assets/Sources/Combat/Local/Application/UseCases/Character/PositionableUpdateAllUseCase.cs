using Combat.Common;
using Combat.Domain.Entities;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

using System;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class PositionableUpdateAllUseCase
    {
        private readonly ICharacterMotionOutput _movementOutput;
        private readonly IPositionableRepository _positionableRepository;

        public PositionableUpdateAllUseCase(ICharacterMotionOutput forcedMovementOutput, IPositionableRepository positionableRepository)
        {
            _movementOutput = forcedMovementOutput;
            _positionableRepository = positionableRepository;
        }

        public void Execute(ReadOnlySpan<UnitContext> targets, float deltaTime)
        {
            foreach (var target in targets)
            {
                HandleTarget(target, deltaTime);
            }
        }

        private void HandleTarget(UnitContext target, float deltaTime)
        {
            Positionable positionable = _positionableRepository.Get(target.Id);
            bool isDirty = positionable.ProgressMovementEffects(deltaTime * target.Attributes.TimeScale);

            var (Directional, Relative) = positionable.GetForcedVelocity();
            System.Numerics.Vector3 desiredVelocity = GetDesiredVelocity(target, positionable);
            _movementOutput.Present(target.Id, Directional, Relative, desiredVelocity);
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
