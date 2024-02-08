using Core.Combat.Abilities;
using Core.Combat.Units.Components;
using Utils.DataTypes;

using ModelData = Utils.DataTypes.UnitCreationData.ModelData;

namespace Core.Combat.Units.Creation
{
    public class UnitFactory
    {
        public Unit Create(int id, ModelData data)
        {
            UnitState state = new UnitState(data.Stats, new(data.CastResourceData), (Team.Team) data.Team, id);
            Spellcasting spellcasting = new Spellcasting();

            Unit unit = new Unit(spellcasting, state);

            foreach (SpellId spellId in data.Spells)
            {
                unit.GiveAbility(Spell.Get(spellId));
            }

            return unit;
        }
    }
}
