using Server.Combat.Data.Entities;

using UnityEngine;

namespace Assets.Sources.Common
{
    [CreateAssetMenu(menuName = "Assets/Skills/SkillPack")]
    public class SkillPack : ScriptableObject
    {
        [SerializeField] private SkillData[] _skills;

        private void OnEnable()
        {
            
        }
    }
}
