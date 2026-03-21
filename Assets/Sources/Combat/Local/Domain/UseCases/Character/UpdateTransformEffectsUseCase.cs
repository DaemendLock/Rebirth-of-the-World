using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public interface IMovementEffectRepository
    {
        void Create(MoveInDirectionEffect value);
        void Update(MoveInDirectionEffect value);
        void Delete(MoveInDirectionEffect value);
    }

    public class AddMovementEffectUseCase
    {
        public void Execute(EntityId targetId, Vector3 direction, float speed, bool relative)
        {
            Vector3 velocity = direction.normalized * speed;
            MoveInDirectionEffect effect = new(targetId, velocity, relative);

            
        }
    }

    public class UpdateTransformEffectsUseCase
    {
        private readonly IPositionableRepository _positionableRepository;
        private readonly ICharacterUpdateList _characterUpdateList;

        public UpdateTransformEffectsUseCase(IPositionableRepository positionableRepository, ICharacterUpdateList characterUpdateList)
        {
            _positionableRepository = positionableRepository;
            _characterUpdateList = characterUpdateList;
        }

        public void Execute(EntityId targetId, float deltaTime)
        {
            Positionable positionable = _positionableRepository.Get(targetId);
            Updatable updatable = _characterUpdateList.Get(targetId);

            deltaTime *= updatable.TimeScale;

            Vector3 velocity = Vector3.zero;

            foreach (var effect in positionable.GetMoveInDirectionOverTimeEffects())
            {
                if (effect.IsRelative)
                {
                    velocity += positionable.Rotation * effect.Velocity * positionable.Scale;
                }

                velocity += effect.Velocity;
            }

            positionable.Position += velocity * deltaTime;

            float scale = 0;

            foreach (var effect in positionable.GetScaleOverTimeEffects())
            {
                scale += (effect.Rate / 100f);
            }

            positionable.Scale += scale * deltaTime;

            _positionableRepository.Update(positionable);
        }
    }
}
