using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Components
{
    [RequireComponent(typeof(Camera))]
    public class PlayerModelComponent : MonoBehaviour
    {
        [SerializeField] private float _distance = 1.0f;
        [SerializeField] private float _height = 2.0f;

        [field: SerializeField] public Transform FollowTarget { get; set; }

        public UnitId? FollowId { get; set; }

        private void OnValidate()
        {
            if (transform.parent != FollowTarget)
            {
                Follow(FollowTarget);
            }
        }

        public void Follow(Transform target)
        {
            if (FollowTarget == target)
            {
                return;
            }

            FollowTarget = target;
            transform.parent = FollowTarget;

            if (target == null)
            {
                return;
            }

            Vector3 shift = new(0, _height, -_distance);
            transform.position = target.position + (target.rotation * shift);
        }

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
