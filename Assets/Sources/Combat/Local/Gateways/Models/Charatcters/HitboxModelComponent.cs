using Combat.Common.Primitives;
using Combat.Domain.Events;

using System;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    [RequireComponent(typeof(Collider))]
    public class HitboxModelComponent : MonoBehaviour
    {
        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
        }

        private void OnDestroy()
        {
            Hitted = null;
        }

        public event Action<HitEvent> Hitted;

        public HitboxType Type { get; set; }

        public UnitId Owner { get; set; }

        public void OnTriggerEnter(Collider other)
        {
            if (enabled == false)
            {
                return;
            }

            if (other.TryGetComponent(out HurtboxModelComponent hurtbox) == false)
            {
                return;
            }

            if (hurtbox.enabled == false)
            {
                return;
            }

            Vector3 location = transform.position;
            Hitted?.Invoke(new(Owner, Type, hurtbox.Owner, hurtbox.Type, new(location.x, location.y, location.z)));
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
            {
                return;
            }

            if (collision.collider.TryGetComponent(out HurtboxModelComponent hurtbox) == false)
            {
                return;
            }

            if (collision.contactCount == 0)
            {
                return;
            }

            Vector3 location = collision.GetContact(0).point;
            Hitted?.Invoke(new(Owner, Type, hurtbox.Owner, hurtbox.Type, new(location.x, location.y, location.z)));
        }
    }
}
