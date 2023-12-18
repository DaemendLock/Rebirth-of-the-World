using Core.Combat.Abilities;
using Data.Spells;
using Data.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.DataTypes;

namespace Assets.Sources.Temp
{
    internal class DataIniter : MonoBehaviour
    {
        [SerializeField] private AssetLabelReference _charactersLabel;
        [SerializeField] private AssetLabelReference _itemLabel;

        private void Start()
        {
            LoadSpellLib();
            LoadCharacters();
            LoadItems();
            //ItemLib.LoadAllData();
            //ModelLib.LoadAllData();
            //SpriteLib.LoadAllData();
        }

        private static void LoadSpellLib()
        {
            //Paladin

            SpellDataLoader.Load();

            SpellId[] ids = SpellDataLoader.GetLoadedIds();

            foreach (SpellId id in ids)
            {
                Spell.Get(id);
            }
        }

        private void LoadCharacters()
        {
            Addressables.LoadAssetsAsync<Loadable>(_charactersLabel, (callback)
                =>
            {
                callback.OnLoad();
            });

        }

        private void LoadItems()
        {
            Addressables.LoadAssetsAsync<Loadable>(_itemLabel, (callback)
                =>
            {
                callback.OnLoad();
            });

        }
    }
}
