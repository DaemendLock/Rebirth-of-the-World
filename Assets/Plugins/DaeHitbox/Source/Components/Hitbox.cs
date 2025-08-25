using System;

using UnityEngine;

namespace DaeHitbox.Components
{
    [RequireComponent(typeof(Collider))]
    public abstract class Hitbox : MonoBehaviour, IHitbox
    {
        private Collider _collider;
        private int _activeCounter;

        public event Action<HitEvent> Hitted;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _collider.enabled = enabled;
        }

        private void OnDisable()
        {
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TryHandleCollsition(other, out HitEvent @event) == false)
            {
                return;
            }

            Hitted?.Invoke(@event);
        }

        [field: SerializeField] public HitboxType HitboxType { get; }

        public void SetActive(bool active)
        {
            if (active)
            {
                _activeCounter++;

                if (_activeCounter == 1)
                {
                    enabled = true;
                }
            }
            else
            {
                if (_activeCounter == 0)
                {
                    throw new InvalidOperationException("Can't set hitbox as inactive.");
                }

                _activeCounter--;
            }

            enabled = _activeCounter > 0;
        }

        protected abstract bool TryHandleCollsition(Collider other, out HitEvent hitEvent);
    }
}