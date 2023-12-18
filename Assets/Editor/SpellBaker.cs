using Core.Combat.Abilities;
using Core.SpellLib.Warrior;
using Data.DataMapper;
using Data.Spells;
using SpellLib.Paladin;
using SpellLib.Weapons;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.DataTypes;
using Utils.Serializer;

namespace Assets.Editor
{
#if UNITY_EDITOR
    public static class SpellBaker
    {
        private static readonly string _PATH = Application.streamingAssetsPath + $"{System.IO.Path.DirectorySeparatorChar}Spells{System.IO.Path.DirectorySeparatorChar}spells.datamap";
        private static readonly string _PATH_COMBAT = Application.streamingAssetsPath + $"{System.IO.Path.DirectorySeparatorChar}Spells{System.IO.Path.DirectorySeparatorChar}combatspells.data";

        private static HashSet<Spell> _target = new()
        {
            //Weapons
            new DefaultSwordAttack(),

            //Warrior
            // > Spec 1
            new DirectHit(),
            new СoncentratedDefense(),
            new IgnorPain(),
            // > Spec 2

            // > Spec 3

            // Paladin
            // > Spec 1
            new LifegivingLight(),
             new BladeOfFaith(),
             new BladeOfFaithProc(),
             new BladeOfFaithProcSelf(),
             new Consecration(),
             new ConsecrationAllyBuff(),
             new ConsecrationEnemyDamage(),
             new ConsecrationEnemyHit(),
             new CandentArmor(),
             new CandentArmorProc(),
             new CandentArmorProcPower(),
            // > Spec 2
            // > Spec 3

            //Shielder
            // > Spec 1
            // > Spec 2
            // > Spec 3
        };

        private static List<SpellId> _ids = new();
        private static List<MappedData> _combat = new();

        [MenuItem("Assets/Bake Spells")]
        public static void Bake()
        {
            _ids.Clear();
            _combat.Clear();

            int position = 0;

            using (BinaryWriter combat = new BinaryWriter(File.OpenWrite(_PATH_COMBAT)))
            {
                foreach (Spell spellTarget in _target)
                {
                    byte[] bytes = SpellSerializer.Serialize(spellTarget.Data);

                    MappedData data = new MappedData(position, bytes.Length);
                    _ids.Add(spellTarget.Id);
                    _combat.Add(data);

                    position += bytes.Length;
                    combat.Write(bytes, 0, bytes.Length);
                }
            }

            using BinaryWriter id = new BinaryWriter(File.OpenWrite(_PATH));

            for (int i = 0; i < _ids.Count; i++)
            {
                id.Write(_ids[i]);
                id.Write(_combat[i].Position);
                id.Write(_combat[i].Size);
            }

            Debug.Log($"Successfully baked {_ids.Count} spells...");
        }

        [MenuItem("Assets/Load Spell Library")]
        public static void Load()
        {
            SpellDataLoader.Load();
            SpellId[] ids = SpellDataLoader.GetLoadedIds();

            foreach (SpellId id in ids)
            {
                Spell.Get(id);
            }

            SpellDataLoader.Clear();
        }

        [MenuItem("Assets/Release loaded data")]
        public static void Release()
        {
            Spell.ReleaseLoadedSpells();
            SpellDataLoader.Clear();
        }
    }
#endif
}
