using UnityEngine;

namespace Client.Combat.Presentation
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : BindableViewComponent<Transform>, ICameraView
    {
        private const float AngleLimit = 75;

        [SerializeField] private float _height;
        [SerializeField] private float _distance;
        [SerializeField, Range(-AngleLimit, AngleLimit)] private float _horizontalRotation;

        public Vector2 Rotation { get => new(0, _horizontalRotation); set => _horizontalRotation = Mathf.Clamp(value.y, -AngleLimit, AngleLimit); }

        private void LateUpdate()
        {
            if (Model == null)
                return;

            transform.position = Model.position + _height * Vector3.up * Model.localScale.x - Quaternion.AngleAxis(-_horizontalRotation, Model.right) * Model.forward * _distance * Model.localScale.x;
            transform.forward = (Quaternion.AngleAxis(-_horizontalRotation, Model.right) * Model.forward);
        }
    }
}
