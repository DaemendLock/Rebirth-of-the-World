using Core.Combat.Abilities;
using Data.Spells;
using Data.Utils;
using SpellLib.Paladin;
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
            Spell.RegisterSpell(new LifegivingLight());
            Spell.RegisterSpell(new BladeOfFaith());
            Spell.RegisterSpell(new BladeOfFaithProc());
            Spell.RegisterSpell(new BladeOfFaithProcSelf());
            Spell.RegisterSpell(new Consecration());
            Spell.RegisterSpell(new ConsecrationAllyBuff());
            Spell.RegisterSpell(new ConsecrationEnemyDamage());
            Spell.RegisterSpell(new ConsecrationEnemyHit());
            Spell.RegisterSpell(new CandentArmor());
            Spell.RegisterSpell(new CandentArmorProc());
            Spell.RegisterSpell(new CandentArmorProcPower());

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
