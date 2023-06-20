using Combat.SpellOld;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat {
    public class SpellCard : MonoBehaviour
    {
        public int spellNum = 0;
        public UI globalUI;
        public Image spellIcon;
        public Text cdText;
        public Image effect;
        private OldAbility curAbility;

        public void InsertAbility(OldAbility ability)
        {
            curAbility = ability;
            if (ability == null)
            {
                spellIcon.sprite = null;
                spellIcon.gameObject.SetActive(false);
                cdText.gameObject.SetActive(false);
                effect.gameObject.SetActive(false);
                return;
            }
            spellIcon.sprite = ability.AbilityIcon;
            spellIcon.gameObject.SetActive(true);
            cdText.gameObject.SetActive(true);
            effect.gameObject.SetActive(true);
        }

        public void CastSpell() => Controller.Instance.SelectedUnit?.GetAbilityByIndex(spellNum).CastAbility();

        public void UpdateCd() =>
            effect.fillAmount = (curAbility != null && curAbility.AbilityCooldown != 0) ? curAbility.GetActualCooldown() / curAbility.AbilityCooldown : 0;
    }
}