using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public sealed class ProjectileModel : MonoBehaviour
    {
        public UnitId? Owner { get; set; }

        public Vector3 Speed { get; set; }
    }
}
