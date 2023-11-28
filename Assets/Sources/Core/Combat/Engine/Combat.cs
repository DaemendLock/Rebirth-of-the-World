using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using Core.Combat.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using Utils.DataTypes;

namespace Core.Combat.Engine
{
    using EntityId = System.Int32;

    internal interface Updatable
    {
        void Update();
    }

    public static class Combat
    {
        private static readonly Dictionary<EntityId, Unit> _units = new(64);

        public static bool Running { get; private set; } = false;

#if DEBUG
        public static event Action<object> DebugMessage;

        internal static void PostDebugMessage(string message)
        {
            DebugMessage?.Invoke(message);
        }
#endif
        public static void Start()
        {
            if (Running)
            {
                return;
            }

            Running = true;

            /*while (_running)
            {
                Update();
            }

            Reset();*/
        }

        public static void Stop()
        {
            if (Running == false)
            {
                return;
            }

            Running = false;
        }

        public static void Reset()
        {
            ModelUpdate.Clear();
            _units.Clear();

            CombatTime.Reset();
        }

        public static void CreateUnit(UnitCreationData data)
        {
            Spellcasting spellcasting = new Spellcasting();

            Unit unit = new(spellcasting, new UnitState(data.Model.Stats, new CastResources(data.Model.CastResourceData),
                new Position(data.Model.PositionData), (Team.Team) data.Model.Team, data.Id));

            foreach (SpellId id in data.Model.Spells)
            {
                spellcasting.GiveAbility(Spell.Get(id));
            }

            foreach (int id in data.Model.Gear)
            {
                unit.Equip(ItemLibrary.ItemLib.GetItem(id));
            }

            RegisterUnit(unit);
        }

        public static void MoveUnit(int unitId, Vector3 position, Vector3 moveDirection, float rotation)
        {
            if (_units.ContainsKey(unitId) == false)
            {
                return;
            }

            Unit unit = _units[unitId];

            unit.Position = position;
            unit.MoveDirection = moveDirection;
            unit.Rotation = rotation;
        }

        public static void CastAbility(EntityId unitId, EntityId targetId, SpellSlot slot)
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

        public static void StopAllActions(int unitId)
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

        public static void StartAttack(int unitId, int target)
        {
            Unit unit;

            if (_units.TryGetValue(unitId, out unit) == false)
            {
                return;
            }

            unit.Target = _units.GetValueOrDefault(target, null);
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

        public static float GetManaRestoreRate() => 0.1f;
        public static float GetEnergyRechargeRate() => 15f;
        public static float GetConcentrationRestoreRate() => 100f;

        //TODO: try move position to array
        internal static List<Unit> FindUnitsInRadius(Vector3 location, float radius, bool includeDead = false, Team.Team excludeTeam = Team.Team.NONE)
        {
            //IsAlive IsInSearchTeam
            List<Unit> result = new();

            foreach (Unit unit in _units.Values)
            {
                if (((includeDead || unit.Alive) == false) ||
                    ((excludeTeam != Team.Team.NONE) && (unit.Team == excludeTeam)) ||
                    ((unit.Position - location).sqrMagnitude > radius * radius))
                {
                    continue;
                }

                result.Add(unit);

            }

            return result;
        }

        private static void RegisterUnit(Unit unit)
        {
            if (_units.ContainsKey(unit.Id))
            {
                ModelUpdate.Remove(unit);
            }

            ModelUpdate.Add(unit);

            lock (_units)
            {
                _units.Add(unit.Id, unit);
            }
        }
    }
}
