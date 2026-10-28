using UnityEngine;

namespace Combat.Local.Presentation.Views
{
    [RequireComponent(typeof(CharacterView), typeof(Rigidbody))]
    public class PositionableView : MonoBehaviour
    {
        private CharacterView _model;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _model = GetComponent<CharacterView>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public Vector3 DirectionalVelocity { get; set; }
        public Vector3 RelativeVelocity { get; set; }
        public Vector3 DesiredVelocity { get; set; }

        private void LateUpdate()
        {
            Vector3 shift = (transform.rotation * (RelativeVelocity + DesiredVelocity)) + DirectionalVelocity;
            _rigidbody.linearVelocity = shift * _model.TimeScale + (Vector3.up * _rigidbody.linearVelocity.y);

            if (_rigidbody.isKinematic)
            {
                _rigidbody.position += _model.TimeScale * Time.deltaTime * shift;
            }
        }
    }
}
