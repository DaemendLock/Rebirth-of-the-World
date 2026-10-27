using UnityEngine;

namespace Combat.Local.Presentation.Views
{
    [RequireComponent(typeof(CharacterViewComponent), typeof(Rigidbody))]
    public class MovableViewComponent : MonoBehaviour
    {
        private CharacterViewComponent _model;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _model = GetComponent<CharacterViewComponent>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public Vector3 DirectionalVelocity { get; set; }
        public Vector3 RelativeVelocity { get; set; }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime * _model.TimeScale;
            Vector3 shift = (transform.rotation * RelativeVelocity) + DirectionalVelocity;
            _rigidbody.position += shift * deltaTime;
        }
    }
}
