using DaeHitbox;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units.Components
{
    [RequireComponent(typeof(Collider))]
    public class Hurtbox : MonoBehaviour, IHurtbox
    {
        private void Awake()
        {
            Owner = GetComponent<Collider>()?.attachedRigidbody.GetComponent<HurtboxOwner>() ?? GetComponentInParent<HurtboxOwner>() ??
                throw new System.ArgumentNullException();
        }

        [field: SerializeField] public HurtboxType HurtboxType { get; private set; }

        public IHurtboxOwner Owner { get; private set; }
    }
}
