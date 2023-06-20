using Remaster.Data;
using Remaster.UnitComponents;
using Remaster.Utils;

namespace Remaster.CombatSetup
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(UnitData data, Team team, Position position)
        {
            Spellcasting spellcasting = new Spellcasting();
            UnitState stats = new UnitState(data.StatsTable, data.Resources, team);

            Unit result = new Unit(spellcasting, stats);

            GiveBasicSpells(spellcasting, data);
            GiveTalentSpells(spellcasting, data);

            Combat.Combat.Instance.RegisterUnit(result);

            return result;
        }

        private static void GiveBasicSpells(Spellcasting spellcasting, UnitData data)
        {
            foreach (Spell spell in data.BasicSpells)
            {
                spellcasting.GiveAbility(spell);
            }
        }

        private static void GiveTalentSpells(Spellcasting spellcasting, UnitData data)
        {
            foreach (Spell spell in data.TalentSpells)
            {
                spellcasting.GiveAbility(spell);
            }
        }
    }
}
