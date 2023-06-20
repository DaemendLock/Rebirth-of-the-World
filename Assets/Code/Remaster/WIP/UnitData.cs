using Remaster.Stats;
using Remaster.UnitComponents;
using System;

namespace Remaster.Data
{
    [Serializable]
    public class UnitData
    {
        public Spell[] BasicSpells = new Spell[0];
        public Spell[] TalentSpells = new Spell[0];
        public int[] ActiveTalents;

        public StatsTable StatsTable;

        public CastResources Resources;
        
    }
}
