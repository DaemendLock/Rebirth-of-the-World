using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.ValueObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Domain.Entities
{
    public ref struct Positionable
    {
        public readonly List<MoveInDirectionEffect> MovementEffects;
        public readonly List<ScaleOverTimeEffect> ScaleEffects;

        public Positionable(UnitId id, Vector3 position, Quaternion rotation, float scale, Quaternion lookDiration, ModelName modelName, Team team, List<MoveInDirectionEffect> movementEffects, List<ScaleOverTimeEffect> scaleEffects)
        {
            Id = id;
            ModelName = modelName;
            Scale = scale;

            Position = position;
            Rotation = rotation;
            LookDirection = lookDiration;
            Team = team;
            Speed = 0f;
            MovementEffects = movementEffects;
            ScaleEffects = scaleEffects;
        }

        public UnitId Id { get; }

        public float Speed { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public Quaternion LookDirection { get; set; }

        public ModelName ModelName { get; }

        public float Scale { get; set; }

        public Team Team { get; set; }

        public readonly void ApplyForcedMovement(MoveInDirectionEffect effect)
        {
            MovementEffects.Add(effect);
        }

        public readonly bool ProgressMovementEffects(float deltaTime)
        {
            for (int i = 0; i < MovementEffects.Count; i++)
            {
                MoveInDirectionEffect effect = MovementEffects[i];
                float newDuration = effect.MaxDuration - deltaTime;
                MovementEffects[i] = new(effect.Velocity, effect.IsRelative, newDuration);
            }

            return MovementEffects.RemoveAll(value => value.MaxDuration <= 0) > 0;
        }

        public readonly (System.Numerics.Vector3 Directional, System.Numerics.Vector3 Relative) GetForcedVelocity()
        {
            System.Numerics.Vector3 Directional = System.Numerics.Vector3.Zero;
            System.Numerics.Vector3 Relative = System.Numerics.Vector3.Zero;

            foreach (var effect in MovementEffects)
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
            ScaleEffects.Add(effect);
        }

        public readonly float GetScaleRate()
        {
            float result = 0;

            foreach (var effect in ScaleEffects)
            {
                result += effect.Rate;
            }

            return result;
        }

        public bool StopScaleOverTime(ScaleEffectId effectId) => ScaleEffects.RemoveAll(value => value.Id == effectId) > 0;
    }
}
