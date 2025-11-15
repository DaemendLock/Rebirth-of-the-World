using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.0f;
        [SerializeField] private float _height = 1.0f;

        [field: SerializeField] public Transform FollowTarget { get; set; }

        private void LateUpdate()
        {
            if (FollowTarget == null)
            {
                return;
            }

            transform.position = FollowTarget.position + (FollowTarget.rotation * new Vector3(0, _height, -_distance));
        }
    }
}
