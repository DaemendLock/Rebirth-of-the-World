using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CharacterModelComponent : MonoBehaviour
    {
        [SerializeField] private string _modelName;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_rigidbody.isKinematic)
            {
                //_rigidbody.position += Time.deltaTime * TimeScale * Velocity;
            }
        }

        public Quaternion LookDirection { get; set; }

        public UnitId Id { get; set; }

        public ModelName ModelName { get => new(_modelName); set => _modelName = value.Value; }

        public float TimeScale { get; set; }

        public byte TeamId { get; set; }

        public float Speed { get; set; }

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        public List<MoveInDirectionEffect> ForcedMovements { get; set; }

        public List<ScaleOverTimeEffect> ScaleEffects { get; set; }
    }
}
