using Core.Combat.Abilities;
using Core.Combat.Engine;
using Core.Combat.Team;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Engine
{
    using EntityId = System.Int32;

    internal static class Units
    {
        private static readonly List<Unit> _units = new(64);
        private static readonly List<Position> _positions= new List<Position>(64);

        private static Queue<EntityId> _avaibleSlots= new Queue<EntityId>();

        #region Createion/Deletion
        public static void CreateUnit(int unitId, UnitCreationData.ModelData data)
        {
            Spellcasting spellcasting = new Spellcasting();

            Unit unit = new(spellcasting, new UnitState(data.Stats, new CastResources(data.CastResourceData),
                new Position(data.PositionData), (Team.Team) data.Team, unitId));

            foreach (SpellId id in data.Spells)
            {
                spellcasting.GiveAbility(Spell.Get(id));
            }

            RegisterUnit(unit);
        }
        #endregion

        #region Positions
        internal static List<Unit> FindUnitsInRadius(Vector3 location, float radius, Team.Team team = Team.Team.NONE, bool includeDead = false)
        {
            List<Unit> result = new();

            for (EntityId i = 0; i < _units.Count;i++)
            {
                if(UnitInRadius(i, location, radius) == false)
                {
                    continue;
                }

                if (UnitMatchFilters(_units[i], includeDead, team) == false)
                {
                    continue;
                }

                result.Add(_units[i]);
            }

            return result;
        }

        public static void SetPosition(EntityId index, Position position)
        {
            _positions[index] = position;
        }

        public static Position GetPosition(EntityId index)
        {
            return _positions[index];
        }
        #endregion
        
        private static void RegisterUnit(Unit unit)
        {
            ModelUpdate.Add(unit);

            lock (_units)
            {
                _units.Add(unit);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool UnitInRadius(EntityId id, Vector3 location, float radius)
        {
            return (location - _positions[id].Location).sqrMagnitude <= (radius * radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool UnitMatchFilters(Unit unit, bool includeDead, Team.Team team)
        {
            bool matchAlive = includeDead || unit.Alive;
            bool matchTeam = (team & unit.Team) != 0;

            return matchAlive && matchTeam;
        }
    }
}
