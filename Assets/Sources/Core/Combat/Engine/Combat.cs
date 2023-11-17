using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using Syncronization;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Engine
{
    using EntityId = System.Int32;

    public interface Updatable
    {
        void Update();
    }

    public static class Combat
    {
        private static readonly List<Updatable> _updates = new();
        private static readonly Queue<Updatable> _registerQueue = new();
        private static readonly Dictionary<EntityId, Unit> _units = new(64);

        private static bool _running = false;
        private static long _lastUpdate = 0;

        public static long UpdateTime { get; private set; }

#if DEBUG
        public static event Action<object> DebugMessage;

        internal static void PostDebugMessage(string message)
        {
            DebugMessage?.Invoke(message);
        }
#endif
        public static void Start()
        {
            if (_running)
            {
                return;
            }

            _running = true;
            CombatSyncroniaztion.MoveRequested += ctx => MoveUnit(ctx.UnitId, ctx.Position, ctx.MoveDirection, ctx.Rotation);
            CombatSyncroniaztion.CastRequested += ctx => CastAbility(ctx.UnitId, ctx.TargetId, (SpellSlot) ctx.SpellSlot);
            CombatSyncroniaztion.TargetRequested += ctx => StartAttack(ctx.Attacker, ctx.Target);
            CombatSyncroniaztion.CancelRequested += ctx => StopAllActions(ctx.Unit);

            /*while (_running)
            {
                Update();
            }

            Reset();*/
        }

        public static void Stop()
        {
            if (_running == false)
            {
                return;
            }

            CombatSyncroniaztion.MoveRequested -= ctx => MoveUnit(ctx.UnitId, ctx.Position, ctx.MoveDirection, ctx.Rotation);
            CombatSyncroniaztion.CastRequested -= ctx => CastAbility(ctx.UnitId, ctx.TargetId, (SpellSlot) ctx.SpellSlot);
            CombatSyncroniaztion.TargetRequested -= ctx => StartAttack(ctx.Attacker, ctx.Target);
            CombatSyncroniaztion.CancelRequested -= ctx => StopAllActions(ctx.Unit);

            _running = false;
        }

        public static void Reset()
        {
            _updates.Clear();
            _units.Clear();

            CombatTime.Reset();
        }

        public static void Update(float deltaTime)
        {
            UpdateTime = (long) (deltaTime * 1000);

            UpdateModel();
            UpdateUpdateble();
            RegisterUpdateable();

            _lastUpdate = CombatTime.Time;
        }

        public static void CreateUnit(UnitCreationData data)
        {
            Unit unit = new(data.Spellcasting, data.UnitState);
            RegisterUnit(unit);
        }

        internal static void RegisterUnit(Unit unit)
        {
            if (_units.ContainsKey(unit.Id))
            {
                lock (_updates)
                {
                    _updates.Remove(unit);
                }
            }

            _registerQueue.Enqueue(unit);

            lock (_units)
            {
                _units.Add(unit.Id, unit);
            }
        }

        public static Vector3 GetUnitPosition(EntityId id)
        {
            if (_units.ContainsKey(id) == false)
            {
                return Vector3.zero;
            }

            return _units[id].Position;
        }

        public static Unit GetUnit(EntityId id)
        {
            return _units.GetValueOrDefault(id);
        }

        //TODO: try move position to array
        public static List<Unit> FindUnitsInRadius(Vector3 location, float radius, bool includeDead = false, Team.Team excludeTeam = Team.Team.NONE)
        {
            //IsAlive IsInSearchTeam
            List<Unit> result = new();

            foreach (Unit unit in _units.Values)
            {
                if ((unit.Position - location).sqrMagnitude > radius * radius)
                {
                    continue;
                }

                if ((includeDead || unit.Alive) && (excludeTeam == Team.Team.NONE || (excludeTeam & unit.Team) != 0))
                {
                    result.Add(unit);
                }
            }

            return result;
        }

        public static float GetManaRestoreRate() => 0.1f;
        public static float GetEnergyRechargeRate() => 15f;
        public static float GetConcentrationRestoreRate() => 100f;

        private static void UpdateModel()
        {
            //ModelSync.Synchronize();
        }

        private static void UpdateUpdateble()
        {
            foreach (Updatable updatable in _updates)
            {
                updatable.Update();
            }
        }

        private static void RegisterUpdateable()
        {
            if (_registerQueue.Count == 0)
            {
                return;
            }

            lock (_registerQueue)
            {
                _updates.Capacity = _updates.Count + _registerQueue.Count;

                for (int i = _updates.Count; i < _updates.Capacity; i++)
                {
                    _updates.Add(_registerQueue.Dequeue());
                }
            }
        }

        private static void MoveUnit(EntityId unitId, Vector3 position, Vector3 direction, float rotation)
        {
            if (_units.ContainsKey(unitId) == false)
            {
                return;
            }

            Unit unit = _units[unitId];

            unit.Position = position;
            unit.MoveDirection = direction;
            unit.Rotation = rotation;
        }

        private static void CastAbility(EntityId unitId, EntityId targetId, SpellSlot slot)
        {
            Unit caster = _units[unitId];
            Spell spell = caster.GetAbility(slot)?.Spell;

            if (spell == null)
            {
                return;
            }

            Unit target = null;
            if (targetId != -1)
            {
                target = _units[targetId];
            }

            caster.CastAbility(new CastEventData(caster, target, spell));
        }

        private static void StopAllActions(int unitId)
        {
            Unit unit;

            if (_units.TryGetValue(unitId, out unit) == false)
            {
                return;
            }

            unit.MoveDirection = Vector3.zero;
            unit.Interrupt(new InterruptData(true, null, 0));
            unit.Target = null;
        }

        private static void StartAttack(int unitId, int target)
        {
            Unit unit;

            if (_units.TryGetValue(unitId, out unit) == false)
            {
                return;
            }

            unit.Target = _units.GetValueOrDefault(target, null);
        }
    }
}
