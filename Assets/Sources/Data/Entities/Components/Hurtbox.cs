using Combat.Common.Primitives;

using UnityEngine;

namespace Data.Entities.Components
{
    [RequireComponent(typeof(Collider))]
    public class Hurtbox : MonoBehaviour
    {
        [SerializeField] private string _hurtboxType;

        public HurtboxType Type => new(_hurtboxType);
    }
}
