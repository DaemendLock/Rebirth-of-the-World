using Core.Combat.Abilities;
using Core.Combat.Utils.Serialization;
using Data.DataMapper;
using Data.Spells;
using SpellLib.Paladin;
using SpellLib.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Editor
{
#if UNITY_EDITOR
    public static class SpellBaker
    {
        private static readonly string _PATH = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}spells.datamap";
        private static readonly string _PATH_COMBAT = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}combatspells.data";

        private static readonly string _PATH_LIB = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}general.spelllib";

        private static HashSet<Spell> _target = new()
        {
            //Weapons
            new DefaultSwordAttack(),

            //Warrior
            // > Spec 1
            //new DirectHit(),
            //new СoncentratedDefense(),
            //new IgnorPain(),
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

        [MenuItem("Data/Bake Spells")]
        public static void Bake()
        {
            _ids.Clear();
            _combat.Clear();

            string bakeLogName = $"Logs{Path.DirectorySeparatorChar}spellbake-{DateTime.UtcNow.ToFileTimeUtc()}.log";

            using StreamWriter log = new StreamWriter(File.OpenWrite(bakeLogName));

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

            using (BinaryWriter id = new BinaryWriter(File.OpenWrite(_PATH)))
            {
                for (int i = 0; i < _ids.Count; i++)
                {
                    id.Write(_ids[i]);
                    id.Write(_combat[i].Position);
                    id.Write(_combat[i].Size);

                    log.WriteLine($"Baked spell(Id:{_ids[i]}): Position - {_combat[i].Position}, Size - {_combat[i].Size}");
                }
            }

            using BinaryWriter spelllib = new(File.OpenWrite(_PATH_LIB));

            spelllib.Write((long) _ids.Count);

            using (BinaryReader datamap = new BinaryReader(File.OpenRead(_PATH)))
            {
                datamap.BaseStream.CopyTo(spelllib.BaseStream);
            }

            using (BinaryReader spells = new BinaryReader(File.OpenRead(_PATH_COMBAT)))
            {
                spells.BaseStream.CopyTo(spelllib.BaseStream);
            }

            Debug.Log($"Successfully baked {_ids.Count} spells. Log: {bakeLogName}");
        }

        [MenuItem("Data/Release loaded spells")]
        public static void Release()
        {
            Spell.ReleaseLoadedSpells();
            SpellDataLoader.Release();
        }
    }
#endif
}
