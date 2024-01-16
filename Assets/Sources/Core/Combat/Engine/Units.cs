using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Engine
{
    using EntityId = System.Int32;

    public static class Units
    {
        private static readonly List<Unit> _units = new(64);
        private static readonly List<Position> _positions = new List<Position>(64);

        private static Queue<EntityId> _avaibleSlots = new Queue<EntityId>();

        #region Creation/Deletion
        public static EntityId GetAvaibleId()
        {
            if (_avaibleSlots.Count == 0)
            {
                return _units.Count;
            }

            return _avaibleSlots.Peek();
        }

        public static Unit GetUnit(EntityId id)
        {
            if (id < 0 || id >= _units.Count)
                return null;

            return _units[id];
        }

        public static void CreateUnit(EntityId id, UnitCreationData.ModelData data)
        {
            Spellcasting spellcasting = new Spellcasting();

            Unit unit = new(spellcasting, new UnitState(data.Stats, new CastResources(data.CastResourceData),
                (Team.Team) data.Team, id));

            foreach (SpellId spell in data.Spells)
            {
                spellcasting.GiveAbility(Spell.Get(spell));
            }

            RegisterUnit(unit, id);
            _positions[id] = new(data.PositionData);
        }

        public static void Clear()
        {
            _units.Clear();
            _positions.Clear();
            _avaibleSlots.Clear();
        }
        
        public static void RemoveUnit(EntityId id)
        {
            
        }
        #endregion

        #region Positions
        internal static List<Unit> FindUnitsInRadius(Vector3 location, float radius, Team.Team team = Team.Team.NONE, bool includeDead = false)
        {
            List<Unit> result = new();

            for (EntityId i = 0; i < _units.Count; i++)
            {
                if (UnitInRadius(i, location, radius) == false)
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

        #region Commands

        public static void MoveUnit(int unitId, Vector3 position, Vector3 moveDirection, float rotation)
        {
            Unit unit = GetUnit(unitId);

            if (unit == null)
            {
                return;
            }

            unit.Position = position;
            unit.MoveDirection = moveDirection;
            unit.Rotation = rotation;
        }

        public static void CastAbility(EntityId unitId, EntityId targetId, SpellSlot slot)
        {
            Unit caster = GetUnit(unitId);

            if(caster==null)
            {
                return;
            }

            Spell spell = caster.GetAbility(slot)?.Spell;

            if (spell == null)
            {
                return;
            }

            Unit target = null;

            if (targetId != -1)
            {
                target = GetUnit(targetId);
            }

            caster.CastAbility(new CastEventData(caster, target, spell));
        }

        public static void StopAllActions(EntityId unitId)
        {
            Unit unit = GetUnit(unitId);

            if (unit == null)
            {
                return;
            }

            unit.MoveDirection = Vector3.zero;
            unit.Interrupt(new InterruptData(true, null, 0));
            unit.Target = null;
        }

        public static void StartAttack(EntityId unitId, EntityId target)
        {
            Unit unit = GetUnit(unitId);

            if (unit == null)
            {
                return;
            }

            unit.Target = GetUnit(target);
        }
        #endregion

        private static void RegisterUnit(Unit unit, EntityId id)
        {
            if (id >= _units.Count)
            {
                lock (_units)
                {
                    while (_units.Count < id - 1)
                    {
                        _units.Add(default);
                        _positions.Add(default);
                        _avaibleSlots.Enqueue(_units.Count - 1);
                    }

                    _units.Add(unit);
                    _positions.Add(new Position());
                }

                ModelUpdate.Add(unit);
                return;
            }

            if (_units[id] != null)
            {
                ModelUpdate.Remove(_units[id]);
                ModelUpdate.Add(unit);
                _units[id] = unit;
                return;
            }

            lock (_avaibleSlots)
            {
                for(int i = 0; (i < _avaibleSlots.Count) && (_avaibleSlots.Peek() != id); i++)
                {
                    _avaibleSlots.Enqueue(_avaibleSlots.Dequeue());
                }

                _avaibleSlots.Dequeue();
            }

            ModelUpdate.Add(unit);
            _units[id] = unit;
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
