using Core.Combat.Abilities;
using Data.Spells;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Sources.Temp
{
    internal class DataIniter : MonoBehaviour
    {
        private void Start()
        {
            LoadSpellLib();
            //ItemLib.LoadAllData();
            //ModelLib.LoadAllData();
            //SpriteLib.LoadAllData();
        }

        private void LoadSpellLib()
        {
            SpellDataLoader.Load();
            SpellId[] ids = SpellDataLoader.GetLoadedIds();

            foreach (SpellId id in ids)
            {
                Spell.Get(id);
            }
        }
    }
}
