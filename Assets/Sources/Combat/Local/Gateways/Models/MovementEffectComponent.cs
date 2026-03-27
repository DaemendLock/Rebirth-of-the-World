using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public interface IMovementEffectContainer
    {
        void AddEffect(MoveInDirectionEffect effect);
        void RemoveEffect(TransformEffectId effect);
    }

    [RequireComponent(typeof(CharacterModel))]
    public class MovementEffectComponent : MonoBehaviour, IMovementEffectContainer
    {
        private List<MoveInDirectionEffect> _values = new();
        private CharacterModel _model;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _model = GetComponent<CharacterModel>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime * _model.TimeScale;
            Vector3 shift = Vector3.zero;

            for (int i = 0; i < _values.Count; i++)
            {
                MoveInDirectionEffect effect = _values[i];

                if (effect.IsRelative)
                {
                    shift += transform.rotation * effect.Velocity;
                }
                else
                {
                    shift += effect.Velocity;
                }

                _values[i] = new(effect.Id, effect.Target, effect.Velocity, effect.IsRelative, effect.MaxDuration - deltaTime);
            }

            _rigidbody.position += shift * deltaTime;
        }

        public void AddEffect(MoveInDirectionEffect effect)
        {
            if (_values.Any(value => value.Id == effect.Id))
            {
                throw new System.InvalidOperationException();
            }

            _values.Add(effect);
        }

        public void RemoveEffect(TransformEffectId id)
        {
            _values.RemoveAll(value => value.Id == id);
        }
    }
}
