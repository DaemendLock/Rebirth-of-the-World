using Core.Combat.Interfaces;
using Core.Combat.Units;
using UnityEngine;

namespace View.Combat.UI.ActionBar
{
    public class ActionBar : MonoBehaviour, UnitAssignable
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpellCard[] _spellCards;

        private void Update()
        {
            if (_unit == null)
            {
                return;
            }

            foreach(SpellCard card in _spellCards)
            {
                card.UpdateCd(_unit.GCD);
            }
        }

        public void AssignTo(Unit unit)
        {
            _unit = unit;

            if(_unit != null)

            for (int i = 0; i < _spellCards.Length;i++)
            {
                _spellCards[i].UpdateAbility(unit.GetAbility((Core.Combat.Abilities.SpellSlot) i));
                _spellCards[i].UpdateCd(_unit.GCD);
            }
        }
    }
}
