using Data.Characters;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Entity")]
    public class Npc : ScriptableObject
    {
        [SerializeField] private CharacterStats _stats;
        [SerializeField] private CastResourceData _castResourceData;
        [SerializeField] private ViewSet[] _veiwSets;

        public UnitCreationData.CastResourceData CastResources => new UnitCreationData.CastResourceData(_castResourceData.Left.Resource.MaxValue, _castResourceData.Right.Resource.MaxValue, _castResourceData.Left.Type, _castResourceData.Right.Type);

        public Sprite GetCharacterCard(int activeViewSet) => _veiwSets[activeViewSet].CharacterCard;

        public StatsTable GetStatsTable(int level) => _stats.GetStatsForLevel(level);

        public GameObject GetModel(int activeViewSet) => _veiwSets[activeViewSet].Model;

        public Sprite[] GetSpellIcons(int activeViewSet) => _veiwSets[activeViewSet].GetSpellIcons();

        public UnitCreationData GetUnitCreationData(int combatIndex, int team, CharacterState data)
        {
            StatsTable stats = GetStatsTable(data.Level.Level) + data.GetGearStats();

            SpellId[] gearSpells = data.GetGearSpells();
            SpellId[] spellIds = new SpellId[data.Spells.Length + gearSpells.Length];
            data.Spells.CopyTo(spellIds, 0);
            gearSpells.CopyTo(spellIds, data.Spells.Length);

            UnitCreationData.ModelData mdata = new UnitCreationData.ModelData(spellIds, stats, new UnitCreationData.PositionData(), CastResources,
                (byte) (team));
            UnitCreationData.ViewData vdata = new UnitCreationData.ViewData(data.CharacterId, data.ViewSet);
            UnitCreationData udata = new UnitCreationData(combatIndex, mdata, vdata);

            return udata;
        }
    }
}
