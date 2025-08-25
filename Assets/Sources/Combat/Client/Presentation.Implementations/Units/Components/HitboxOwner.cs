using System.Collections;
using System.Collections.Generic;

using DaeHitbox;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units.Components
{
    public class HitboxOwner : MonoBehaviour, IHitboxCollection
    {
        private Dictionary<DaeHitbox.HitboxType, IHitbox> _hitboxes = new();

        private void Awake()
        {
            _hitboxes.Clear();
            Hitbox[] values = GetComponentsInChildren<Hitbox>();
            _hitboxes.EnsureCapacity(values.Length);

            foreach (var value in values)
            {
                _hitboxes.Add(value.Type, value);
            }
        }

        public IHitbox GetHitbox(DaeHitbox.HitboxType type) => _hitboxes[type];

        public IEnumerator<IHitbox> GetEnumerator() => _hitboxes.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _hitboxes.GetEnumerator();

        public IHitbox this[DaeHitbox.HitboxType type] { get => GetHitbox(type); }
    }
}
