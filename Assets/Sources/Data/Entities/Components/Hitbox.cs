using Combat.Common.ValueObjects;

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
