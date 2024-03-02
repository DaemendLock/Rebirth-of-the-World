using Client.Combat.Core.Abilities;
using Client.Combat.Core.Units.Components;
using UnityEngine;

namespace Client.Combat.View.UI.ActionBar
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private SpellCard[] _spellCards;

        private SpellCasting _valueSource;

        private void Update()
        {
            if (_valueSource == null)
            {
                return;
            }

            foreach (SpellCard card in _spellCards)
            {
                card.UpdateCooldown();
            }
        }

        public void AssignTo(SpellCasting spellCasting)
        {
            if (spellCasting == null)
            {
                return;
            }

            for (int i = 0; i < _spellCards.Length; i++)
            {
                Ability ability = _valueSource.GetAbility(i);

                _spellCards[i].SetAbility(ability);
            }
        }
    }
}
