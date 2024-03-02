using Core.Combat.Abilities;
using Core.Combat.Units.Components;
using Server.Combat.Core.Utils;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.Patterns.Factory;
using ModelData = Utils.DataTypes.UnitCreationData.ModelData;

namespace Core.Combat.Units.Creation
{
    using EntityId = System.Int32;

    public class UnitFactory : Factory<Unit, ModelData>
    {
        private readonly EntityList<Unit> _target;
        private readonly List<Position> _positions;

        public UnitFactory(EntityList<Unit> target, List<Position> positions)
        {
            _target = target;
            _positions = positions;
        }

        public Unit Create(ModelData data)
        {
            EntityId id = _target.NextId;

            UnitState state = new UnitState(data.Stats, new(data.CastResourceData), (Team.Team) data.Team, id);
            Spellcasting spellcasting = new Spellcasting();

            Unit unit = new Unit(spellcasting, state);

            foreach (SpellId spellId in data.Spells)
            {
                unit.GiveAbility(Spell.Get(spellId));
            }

            _target.Add(unit);

            Position position = new(data.PositionData.Location, data.PositionData.Rotation);

            if (_positions.Count > unit.Id)
            {
                _positions[unit.Id] = position;
            }
            else
            {
                _positions.Add(position);
            }

            return unit;
        }
    }
}
