using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Controllers.Inputs
{
    public interface IHitboxInput
    {
        event Action<HitboxId, HurtboxId, Vector3> Hitted;
        event Action<HitboxId> Destroied;
    }

    [RequireComponent(typeof(Collider))]
    public class HitboxInputComponent : MonoBehaviour, IHitboxInput
    {
        public event Action<HitboxId, HurtboxId, Vector3> Hitted;
        public event Action<HitboxId> Destroied;

        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            //UnityEngine.Debug.Log($"New hitbox: {gameObject.name}");
        }

        private void OnDestroy()
        {
            Hitted = null;
            Destroied?.Invoke(Id);
            Destroied = null;
        }

        public HitboxId Id { get; set; }

        public void OnTriggerEnter(Collider other)
        {
            if (enabled == false)
            {
                return;
            }

            if (other.TryGetComponent(out HurtboxInputComponent hurtbox) == false)
            {
                return;
            }

            Hitted?.Invoke(Id, hurtbox.Id, transform.position);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
            {
                return;
            }

            if (collision.collider.TryGetComponent(out HurtboxInputComponent hurtbox) == false)
            {
                return;
            }

            if (collision.contactCount == 0)
            {
                return;
            }

            Hitted?.Invoke(Id, hurtbox.Id, collision.GetContact(0).point);
        }

        public override bool Equals(object obj) => obj is HitboxInputComponent model && EqualityComparer<HitboxId>.Default.Equals(Id, model.Id);

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
