using UnityEngine;

namespace Remaster.View
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private Unit unit; 
        [SerializeField] private SpellCard[] _spellCards;

        private void Start()
        {
            for (int i = 0; i < (int)SpellSlot.SIXTH; i++)
            {
                _spellCards[i].Init(unit.GetAbility((SpellSlot)i));
            }
        }
    }
}
