using UnityEngine;

namespace Data.Entities
{
    [RequireComponent(typeof(SkillData))]
    public class SkillCooldownComponent : MonoBehaviour
    {
        [field: SerializeField] public float Value { get; private set; }
    }
}
