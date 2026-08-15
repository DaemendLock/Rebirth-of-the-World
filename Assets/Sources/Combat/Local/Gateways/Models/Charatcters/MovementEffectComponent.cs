using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    [RequireComponent(typeof(CharacterModelComponent))]
    public class MovementEffectComponent : MonoBehaviour
    {
        private readonly List<MoveInDirectionEffect> _values = new();
        private CharacterModelComponent _model;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _model = GetComponent<CharacterModelComponent>();
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

                _values[i] = new(effect.Velocity, effect.IsRelative, effect.MaxDuration - deltaTime);
            }

            _rigidbody.position += shift * deltaTime;
        }

        public ICollection<MoveInDirectionEffect> Effects => _values;
    }
}
