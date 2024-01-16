using Core.Combat.Abilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace View.Combat.UI.ActionBar
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
                _abilityIcon.gameObject.SetActive(value: false);
                _abilityIcon.enabled = false;
                return;
            }

            _ability = ability;
            _cooldown.sprite = _abilityIcon.sprite ?? _background.sprite;
            _abilityIcon.gameObject.SetActive(true);
            _abilityIcon.enabled = true;
        }

        public void UpdateCd(float GCD)
        {
            if (_ability == null)
            {
                return;
            }

            if (_ability.Spell.GcdCategory == GcdCategory.Ignor)
            {
                _cooldown.fillAmount = _ability.Cooldown.FullTime > 0 ? _ability.Cooldown.Left / _ability.Cooldown.FullTime : 0;
                return;
            }

            float activeCooldown = MathF.Max(_ability.CooldownTime, GCD);

            _cooldown.fillAmount = _ability.Cooldown.FullTime > 0 ? activeCooldown / _ability.Cooldown.FullTime : activeCooldown;
        }

        public void SetIcon(Sprite icon)
        {
            _abilityIcon.sprite = icon;
            _cooldown.sprite = icon;
        }
    }
}
