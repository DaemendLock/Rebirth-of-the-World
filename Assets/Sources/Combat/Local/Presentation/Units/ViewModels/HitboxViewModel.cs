using System;
using System.Collections.Generic;

using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Units.ViewModels
{
    [RequireComponent(typeof(Collider))]
    public class HitboxViewModel : MonoBehaviour
    {
        public event Action<HitboxViewModel, HurtboxViewModel, Vector3> Hitted;

        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            //UnityEngine.Debug.Log($"New hitbox: {gameObject.name}");
        }

        private void OnDestroy()
        {
            Hitted = null;
        }

        public HitboxId Id { get; set; }
        public HitboxType Type { get; set; }

        public void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
            {
                return;
            }

            if (collision.collider.TryGetComponent(out HurtboxViewModel hurtbox) == false)
            {
                return;
            }

            if (collision.contactCount == 0)
            {
                return;
            }

            Hitted?.Invoke(this, hurtbox, collision.GetContact(0).point);
        }

        public override bool Equals(object obj) => obj is HitboxViewModel model && EqualityComparer<HitboxId>.Default.Equals(Id, model.Id);
        
        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
