using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine.Services;
using Core.Combat.Units;
using Core.Combat.Units.Creation;
using Core.Combat.Utils;
using System;
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
        //private static readonly List<Resource> _resources = new (64);

        private static readonly Queue<EntityId> _avaibleSlots = new Queue<EntityId>();

        private static UnitFactory _unitFactory = new();

        #region Creation/Deletion
        public static EntityId GetAvaibleId()
        {
            if (_avaibleSlots.Count == 0)
            {
                return _units.Count;
            }

            return _avaibleSlots.Peek();
        }

        public static Unit GetUnitById(EntityId id)
        {
            if (id < 0 || id >= _units.Count)
            {
                return null;
            }

            return _units[id];
        }

        public static void CreateUnit(EntityId id, UnitCreationData.ModelData data)
        {
            Unit unit = _unitFactory.Create(id, data);
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
        public static List<Unit> FindUnitsInRadius(Vector3 location, float radius, Team.Team team = Team.Team.NONE, bool includeDead = false)
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
        public static void MoveUnit(int unitId, Position position, Vector3 moveDirection)
        {
            Unit unit = GetUnitById(unitId);

            if (unit == null)
            {
                return;
            }

            _positions[unitId] = position;
            unit.MoveDirection = moveDirection;
        }

        public static IActionRecordContainer Cast(CastInputData data)
        {
            if (data.Caster == null)
            {
                throw new ArgumentNullException(nameof(data.Caster));
            }

            Ability spell = data.Caster.GetAbility(data.SpellSlot);

            if (spell == null)
            {
                throw new InvalidOperationException("Can't cast from empty slot.");
            }

            CastService castService = new(data.Target, data.Target, spell);

            if (castService.Check() != CommandResult.SUCCES)
            {
                throw new InvalidOperationException(nameof(data));
            }

            return castService.Cast();
        }

        // public static void Cast(Unit unit, SpellSlot slot) => castService.Cast(new CastData());

        public static void StopAllActions(EntityId unitId)
        {
            Unit unit = GetUnitById(unitId);

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
            Unit unit = GetUnitById(unitId);

            if (unit == null)
            {
                return;
            }

            unit.Target = GetUnitById(target);
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
                for (int i = 0; (i < _avaibleSlots.Count) && (_avaibleSlots.Peek() != id); i++)
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
