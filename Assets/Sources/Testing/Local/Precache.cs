using Combat.Local.Data.Entities;

using UnityEngine;

namespace Testing.Local
{
    public class Precache : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        [field: SerializeField] public CastableSkillData[] Skills { get; private set; }
    }
}
