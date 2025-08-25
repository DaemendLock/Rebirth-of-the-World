using UnityEngine;

using UtilsUnity.Patterns.View;

namespace Client.Combat.Presentation.UI.ActionBar
{
    public class ActionBar : MonoBehaviour//, IBindableView<IUnit>
    {
        //[SerializeField] private IBindableView<Ability>[] _spellCards;

        //public void Bind(IUnit unit)
        //{
            //if (unit == null)
            //{
            //    foreach (IBindableView<Ability> card in _spellCards)
            //    {
            //        card.Bind(null);
            //    }

            //    return;
            //}

            //for (int i = 0; i < _spellCards.Length; i++)
            //{
            //    ISkill skill = unit.GetSkillByIndex(i);

            //    if (skill == null)
            //    {
            //        continue;
            //    }

            //    _spellCards[i].Bind(new Ability(unit, skill));
            //}
        //}
    }
}
