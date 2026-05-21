using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Controllers.Inputs
{
    [RequireComponent(typeof(Collider))]
    public class HitboxModelComponent : MonoBehaviour
    {
        private readonly Queue<HitRecord> _hitRecords = new();

        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
        }

        private void OnDestroy()
        {
            _hitRecords.Clear();
        }

        public HitboxType Type { get; set; }

        public UnitId Owner { get; set; }

        public Queue<HitRecord> Records => _hitRecords;

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

            _hitRecords.Enqueue(new(Owner, Type, hurtbox.Owner, hurtbox.Type, transform.position));
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
            _hitRecords.Enqueue(new(Owner, Type, hurtbox.Owner, hurtbox.Type, location));
        }
    }
}
