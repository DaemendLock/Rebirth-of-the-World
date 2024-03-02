using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine.Services;
using Core.Combat.Units;
using Core.Combat.Units.Creation;
using Core.Combat.Utils;
using Server.Combat.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.ThrowHepler;

namespace Core.Combat.Engine
{
    using EntityId = System.Int32;

    public static class Units
    {
        private const int DEFAULT_UNITS_LIST_SIZE = 32;

        private static readonly EntityList<Unit> _units = new(DEFAULT_UNITS_LIST_SIZE);
        private static readonly List<Position> _positions = new(DEFAULT_UNITS_LIST_SIZE);
        private static EntityId[] _filteredUnits = new EntityId[DEFAULT_UNITS_LIST_SIZE];

        private static readonly UnitFactory _unitFactory = new(_units, _positions);

        #region PublicAPI
        public static int Count => _units.Count;

        public static Unit GetUnitById(EntityId id) => _units.Get(id);

        public static void MoveUnit(int unitId, Vector3 moveDirection)
        {
            Unit unit = GetUnitById(unitId);

            //if (unit == null)
            //{
            //    return;
            //}

            unit.MoveDirection = moveDirection;
        }

        public static ActionRecord Cast(CastInputData data)
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

            return castService.CheckCast() ? castService.Cast() : throw new InvalidOperationException("Invalid target.");
        }

        public static void StopAllActions(Unit unit)
        {
            ThrowHepler.ArgumentNullException(unit);

            unit.MoveDirection = Vector3.zero;
            unit.Interrupt(new InterruptData(true, null, 0));
            unit.Target = null;
        }

        public static void StartAttack(Unit attacker, Unit target)
        {
            ThrowHepler.ArgumentNullException(attacker);

            attacker.Target = target;
        }
        #endregion

        internal static Unit[] FindUnitsByPosition(Predicate<Vector3> condition)
        {
            Unit[] result;

            lock (_filteredUnits)
            {
                if (_positions.Capacity != _filteredUnits.Length)
                {
                    _filteredUnits = new EntityId[_positions.Capacity];
                }

                int count = 0;

                for (int i = 0; i < _positions.Count; i++)
                {
                    if (condition.Invoke(_positions[i].Location))
                    {
                        _filteredUnits[count++] = i;
                    }
                }

                result = new Unit[count];

                for (int i = 0; i < count && _filteredUnits[i] < _units.Count; i++)
                {
                    result[i] = _units.Get(_filteredUnits[i]);
                }
            }

            return result;
        }

        internal static IEnumerable<Unit> FindUnitsInRadius(Vector3 location, float radius, Team.Team team = Team.Team.NONE, bool includeDead = false)
        {
            Unit[] result = FindUnitsByPosition((unitPosition) => (location - unitPosition).sqrMagnitude <= (radius * radius));

            return result.Where((unit) => UnitMatchFilters(unit, includeDead, team));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetPosition(EntityId index, Position position) => _positions[index] = position;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ApplyMovement(EntityId index, Vector3 movement)
        {
            Position position = _positions[index];
            _positions[index] = new Position(position.Location + movement, position.Rotation);
        }

        public static Position GetPosition(EntityId index) => _positions[index];

        public static Unit CreateUnit(UnitCreationData.ModelData data)
        {
            Unit unit = _unitFactory.Create(data);
            return unit;
        }

        internal static void RemoveUnit(EntityId id)
        {
            _units.Remove(id);
        }

        internal static void UpdatePosition(long time, UpdateRecord record)
        {
            foreach (Unit unit in _units)
            {
                UpdateUnitPosition(unit, time, record);
            }
        }

        internal static void Update(IActionRecordContainer container)
        {
            foreach (Unit unit in _units)
            {
                unit.Update(container);
            }
        }

        internal static void UpdateDeathState(IActionRecordContainer container)
        {
            foreach (Unit unit in _units)
            {
                if (unit.CurrentHealth <= 0 && unit.Alive)
                {
                    //container.AddAction(unit.Kill());
                    continue;
                }

                if (unit.CurrentHealth > 0 && !unit.Alive)
                {
                    //container.AddAction(unit.Resurrect());
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool UnitMatchFilters(Unit unit, bool includeDead, Team.Team team)
        {
            if (unit == null)
            {
                return false;
            }

            bool matchAlive = includeDead || unit.Alive;
            bool matchTeam = (team & unit.Team) != 0;

            return matchAlive && matchTeam;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateUnitPosition(Unit unit, long time, UpdateRecord record)
        {
            float speed = StatsEvaluator.EvaluateUnitStat(UnitStat.SPEED, unit).CalculatedValue;

            if (speed == 0)
            {
                return;
            }

            Vector3 movement = unit.MoveDirection * (speed * time / 1000);

            Position currentPosition = _positions[unit.Id];

            _positions[unit.Id] = new Position(movement + currentPosition.Location, currentPosition.Rotation);

            record.AddPositionUpdate(new PositionUpdateRecord(unit.Id, _positions[unit.Id], unit.MoveDirection * speed));
        }
    }

    public readonly struct PositionUpdateRecord
    {
        public readonly int UnitId;
        public readonly Position Position;
        public readonly Vector3 Velocity;

        public PositionUpdateRecord(int unitId, Position newPosition, Vector3 velocity)
        {
            Position = newPosition;
            Velocity = velocity;
            UnitId = unitId;
        }
    }
}
