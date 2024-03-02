using Client.Combat.Core.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Combat.View.UI.ActionBar
{
    public class SpellCard : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private Image _cooldown;
        private Ability _ability;

        public void SetAbility(Ability ability)
        {
            if (ability == null)
            {
                _abilityIcon.gameObject.SetActive(false);
                return;
            }

            _ability = ability;
            _cooldown.sprite = _abilityIcon.sprite ?? _background.sprite;
            _abilityIcon.gameObject.SetActive(true);
            _abilityIcon.enabled = true;
            //TODO: _abilityIcon.sprite = icon;
            //_cooldown.sprite = icon;
        }

        public void UpdateCooldown()
        {
            if (_ability == null)
            {
                return;
            }

            _cooldown.fillAmount = _ability.Cooldown.Expired ? 0 : _ability.Cooldown.Left / _ability.Cooldown.FullTime;
        }
    }
}
