using Remaster.Abilities;
using Remaster.Data.SpriteLib;
using UnityEngine;
using UnityEngine.UI;

namespace Remaster.View
{
    public class SpellCard : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private Image _cooldown;
        private Ability _ability;

        private void Update()
        {
            if (_ability == null)
            {
                return;
            }

            _cooldown.fillAmount = (_ability.Cooldown.FullTime > 0) ? (_ability.Cooldown.Left / _ability.Cooldown.FullTime) : 0;
        }

        public void UpdateAbility(Ability ability)
        {

            if (ability == null)
            {
                _abilityIcon.sprite = null;
                _abilityIcon.enabled = false;
                _cooldown.fillAmount = 0;
                _ability = ability;
                return;
            }

            _ability = ability;
            _abilityIcon.enabled = true;
            _cooldown.fillAmount = _ability.CooldownTime / _ability.CooldownTime;
            _abilityIcon.sprite = SpriteLibrary.GetSpellSprite(ability.Spell.Id) ?? SpriteLibrary.LoadSpell(ability.Spell.Id);
        }
    }
}
