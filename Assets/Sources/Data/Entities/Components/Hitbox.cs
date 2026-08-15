using Combat.Common.Primitives;

using UnityEngine;

namespace Data.Entities.Components
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private string _type;

        public HitboxType Type => new(_type);
    }
}
