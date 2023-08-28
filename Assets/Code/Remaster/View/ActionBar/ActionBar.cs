using UnityEngine;

namespace Remaster.View
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private Unit _unit; 
        [SerializeField] private SpellCard[] _spellCards;

        private void Start()
        {
            
        }

        public void SetActiveUnit(Unit unit)
        {
            _unit = unit;

            for(int i = 0; i < _spellCards.Length; i++)
            {
                  _spellCards[i].UpdateAbility(unit.GetAbility((Remaster.SpellSlot)i));
            }
        }
    }
}
