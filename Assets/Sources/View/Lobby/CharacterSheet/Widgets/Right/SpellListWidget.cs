using UnityEngine;
using Utils.DataTypes;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class SpellListWidget : MonoBehaviour
    {
        [SerializeField] private SpellPreviewWidget[] _spells;

        public void ShowSpells(SpellId[] spells)
        {
            int length = Mathf.Min(spells.Length, _spells.Length);

            for (int i = 0; i < length; i++)
            {
                _spells[i].ShowSpell(spells[i]);
            }
        }
    }
}
