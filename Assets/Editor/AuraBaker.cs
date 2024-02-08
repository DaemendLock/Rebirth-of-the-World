using Core.Combat.Abilities;
using Core.Combat.Statuses.Auras;
using Data.DataMapper;
using Data.Spells;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Editor
{
    public static class AuraBaker
    {
        private static readonly string _PATH = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}aura.datamap";
        private static readonly string _PATH_COMBAT = Application.streamingAssetsPath + $"{Path.DirectorySeparatorChar}Spells{Path.DirectorySeparatorChar}auras.data";

        private static HashSet<Aura> _target = new()
        {

        };

        private static List<SpellId> _ids = new();
        private static List<MappedData> _combat = new();

        [MenuItem("Data/Bake Auras")]
        public static void Bake()
        {
            _ids.Clear();
            _combat.Clear();

            int position = 0;

            using (BinaryWriter combat = new BinaryWriter(File.OpenWrite(_PATH_COMBAT)))
            {
                foreach (Aura auraTarget in _target)
                {
                    byte[] bytes = null;
                    //AuraSerializer.Serialize(auraTarget.Data);

                    MappedData data = new MappedData(position, bytes.Length);
                    _ids.Add(auraTarget.Id);
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

        [MenuItem("Data/Release auras")]
        public static void Release()
        {
            Spell.ReleaseLoadedSpells();
            SpellDataLoader.Release();
        }
    }
}
