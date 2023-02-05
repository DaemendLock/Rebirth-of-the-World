using UnityEngine;

namespace Data {

    public enum AbilityDamageSource {
        FIXED,
        ATK_PERCENT,
        SPELLPOWER_PERCENT,
        OTHER
    }

    [CreateAssetMenu(fileName = "UnnamedAbility", menuName = "New Ability", order = 51)]
    public class AbilityData : ScriptableObject {
        [Header("System data")]
        [SerializeField] private ushort _abilityId;
        [Space(5)]
        [Header("Resources")]
        [SerializeField] private string _abilityName;
        public Sprite icon;
        [Space(5)]
        [SerializeField] private string _description;

        public void OnEnable() {
           RotW.StoreAbilityData(this, _abilityId);
        }

        public AbilityData() {

        }

        public ushort AbilityId => _abilityId;

        public string GetName() => _abilityName;

    }

}
