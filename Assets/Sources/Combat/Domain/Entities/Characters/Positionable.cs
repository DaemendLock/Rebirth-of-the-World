using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Domain.Entities
{
    public struct Positionable
    {
        private readonly List<MoveInDirectionEffect> _movementEffects;
        private readonly List<ScaleOverTimeEffect> _scaleEffects;

        public Positionable(UnitId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName, Team team)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            Rotation = rotation;
            LookDirection = lookDiration;
            Team = team;
            Speed = 0f;
            _movementEffects = new();
            _scaleEffects = new();
        }
        private Positionable(UnitId id, float speed, Vector3 position, Quaternion rotation, Quaternion lookDirection, ModelName modelName, float scale, Team team, List<MoveInDirectionEffect> movementEffects, List<ScaleOverTimeEffect> scaleEffects)
        {
            Id = id;
            Speed = speed;
            Position = position;
            Rotation = rotation;
            LookDirection = lookDirection;
            ModelName = modelName;
            Scale = scale;
            Team = team;
            _movementEffects = movementEffects;
            _scaleEffects = scaleEffects;
        }

        public UnitId Id { get; }

        public float Speed { get; set; }

        public Vector3 Position { get; }

        public Quaternion Rotation { get; }

        public Quaternion LookDirection { get; set; }

        public ModelName ModelName { get; }

        public float Scale { get; set; }

        public Team Team { get; set; }

        public readonly void ApplyForcedMovement(MoveInDirectionEffect effect)
        {
            _movementEffects.Add(effect);
        }

        public readonly bool ProgressMovementEffects(float deltaTime)
        {
            for (int i = 0; i < _movementEffects.Count; i++)
            {
                MoveInDirectionEffect effect = _movementEffects[i];
                float newDuration = effect.MaxDuration - deltaTime;
                _movementEffects[i] = new(effect.Velocity, effect.IsRelative, newDuration);
            }

            return _movementEffects.RemoveAll(value => value.MaxDuration <= 0) > 0;
        }

        public readonly (System.Numerics.Vector3 Directional, System.Numerics.Vector3 Relative) GetForcedVelocity()
        {
            System.Numerics.Vector3 Directional = System.Numerics.Vector3.Zero;
            System.Numerics.Vector3 Relative = System.Numerics.Vector3.Zero;

            foreach (var effect in _movementEffects)
            {
                if (effect.MaxDuration <= 0)
                {
                    continue;
                }

                if (effect.IsRelative)
                {
                    Relative += effect.Velocity;
                }
                else
                {
                    Directional += effect.Velocity;
                }
            }

            return (Directional, Relative);
        }

        public readonly void StartScaleOverTime(ScaleOverTimeEffect effect)
        {
            _scaleEffects.Add(effect);
        }

        public readonly float GetScaleRate()
        {
            float result = 0;

            foreach (var effect in _scaleEffects)
            {
                result += effect.Rate;
            }

            return result;
        }

        public readonly bool StopScaleOverTime(ScaleEffectId effectId) => _scaleEffects.RemoveAll(value => value.Id == effectId) > 0;

        public readonly Positionable With(Vector3 position, Quaternion rotation) =>
            new(Id, Speed, position, rotation, LookDirection, ModelName, Scale, Team, _movementEffects, _scaleEffects);
    }
}
