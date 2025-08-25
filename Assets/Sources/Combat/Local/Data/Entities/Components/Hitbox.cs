using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private string _type;

        public HitboxType Type => new(_type);
    }
}
