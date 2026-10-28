using Combat.Common;
using Combat.Domain.Entities;
using Combat.Local.Application.OutputPorts;

using System;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class PositionableUpdateAllUseCase
    {
        private readonly ICharacterMotionOutput _movementOutput;

        public PositionableUpdateAllUseCase(ICharacterMotionOutput movementOutput)
        {
            _movementOutput = movementOutput;
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
            bool isDirty = target.Positionable.ProgressMovementEffects(deltaTime * target.Attributes.TimeScale);

            var (Directional, Relative) = target.Positionable.GetForcedVelocity();
            System.Numerics.Vector3 desiredVelocity = GetDesiredVelocity(target);
            _movementOutput.Present(target.Id, Directional, Relative, desiredVelocity);
        }

        private System.Numerics.Vector3 GetDesiredVelocity(UnitContext unit)
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

            float speed = unit.Positionable.Speed;
            var velocity = new UnityEngine.Vector3(desiredDirection.x, 0, desiredDirection.y) * speed;
            return new(velocity.x, velocity.y, velocity.z);
        }
    }
}
