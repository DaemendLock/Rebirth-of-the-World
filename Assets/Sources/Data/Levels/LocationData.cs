using UnityEngine;

namespace Data.Levels
{
    public sealed class LocationData : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}
