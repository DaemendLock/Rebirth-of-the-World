using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using Core.Data;

namespace Core.CombatSetup
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(UnitData data, Team.Team team, Position position)
        {
            Spellcasting spellcasting = new Spellcasting();
            UnitState stats = new UnitState(data.StatsTable, data.Resources, team);

            Unit result = new Unit(spellcasting, stats);

            GiveBasicSpells(spellcasting, data);
            GiveTalentSpells(spellcasting, data);

            Combat.Engine.Combat.RegisterUnit(result);

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
