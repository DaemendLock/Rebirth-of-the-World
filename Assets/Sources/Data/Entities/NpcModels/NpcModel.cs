using Data.Entities.NpcModels;
using UnityEngine;

namespace Data.Entities
{
    [RequireComponent(typeof(Animator))]
    public class NpcModel : MonoBehaviour
    {
        [SerializeField] private Hardpoint _leftHandPosition;
        [SerializeField] private Hardpoint _rightHandPosition;
    }
}
