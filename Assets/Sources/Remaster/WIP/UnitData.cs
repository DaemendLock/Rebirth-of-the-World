using Core.Combat.Abilities;
using Core.Combat.Stats;
using Core.Combat.Units.Components;
using System;

namespace Core.Data
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
