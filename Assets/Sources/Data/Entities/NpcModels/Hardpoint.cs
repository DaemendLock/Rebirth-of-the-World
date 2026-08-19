using UnityEngine;

namespace Data.Entities.NpcModels
{
    public sealed class Hardpoint : MonoBehaviour
    {
        [field: SerializeField] public HardpointType Name { get; private set; }
    }

    public enum HardpointType
    {
        None,
        MainHand
    }
}
