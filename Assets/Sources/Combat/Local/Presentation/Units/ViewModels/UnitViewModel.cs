using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Units.ViewModels
{
    public class UnitViewModel : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _baseMass;

        private void Awake()
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _baseMass = _rigidbody.mass;

            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
        }

        public event System.Action Updated;

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        public float Scale
        {
            get => transform.localScale.x;
            set
            {
                transform.localScale = Vector3.one * value;
                _rigidbody.mass = _baseMass * value * value;
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public EntityId EntityId { get; set; }

        public float Rotation { get; set; }

        public bool Alive { get; set; }

        public Action ActiveAction { get; set; }
    }
}
