using Core.Combat.Abilities;
using Core.Combat.Units;
using Server.Combat.Core.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Utils
{
    public static class StatsEvaluator
    {
        private const float HasteEffiency = 0.00075f;
        private const float CritEffiency = 0.05f;
        private const float VersalityEffiency = 0.002f;

        private static StatsRecord _record = new StatsRecord(0, new Dictionary<(int, UnitStat), PercentModifiedValue>());

        public static float EvaluateUnitStatValue(UnitStat stat, Unit unit)
        {
            if (Time.time != _record.ResolveTime)
            {
                _record = new StatsRecord(Time.time, _record.Memory);
            }

            return _record.GetValue(stat, unit).CalculatedValue;
        }

        public static PercentModifiedValue EvaluateUnitStat(UnitStat stat, Unit unit)
        {
            if (Time.time != _record.ResolveTime)
            {
                _record = new StatsRecord(Time.time, _record.Memory);
            }

            return _record.GetValue(stat, unit);
        }

        public static float EvaluateHasteTimeDivider(Unit unit) => (float) Math.Clamp(1 + EvaluateUnitStatValue(UnitStat.HASTE, unit) * HasteEffiency, 0.5, 2);

        public static float EvaluateVersalityMultiplier(Unit unit) => 1 + Math.Max(0, EvaluateUnitStatValue(UnitStat.VERSALITY, unit) * VersalityEffiency);

        public static float EvaluateCritChance(Unit unit) => EvaluateUnitStatValue(UnitStat.CRIT, unit) * CritEffiency;

        public static float EvaluateCastGcd(Spell spell, Unit caster)
        {
            if (caster == null)
            {
                return spell.GCD;
            }

            return spell.GCD / EvaluateHasteTimeDivider(caster);
        }

        public static float EvaluateAbilityCooldown(float baseCooldown, Unit caster, bool useHaste)
        {
            float cooldown = baseCooldown;

            if (caster != null && useHaste)
            {
                cooldown /= EvaluateHasteTimeDivider(caster);
            }

            return cooldown;
        }

        private readonly struct StatsRecord
        {
            public readonly long ResolveTime;
            private readonly Dictionary<(int, UnitStat), PercentModifiedValue> _values;

            public StatsRecord(long resolveTime, Dictionary<(int, UnitStat), PercentModifiedValue> memory)
            {
                ResolveTime = resolveTime;
                _values = memory;
                memory.Clear();
            }

            public Dictionary<(int, UnitStat), PercentModifiedValue> Memory => _values;

            [MethodImpl(MethodImplOptions.NoOptimization)]
            public PercentModifiedValue GetValue(UnitStat stat, Unit unit)
            {
                if (_values.TryGetValue((unit.Id, stat), out PercentModifiedValue stats))
                {
                    return stats;
                }

                _values[(unit.Id, stat)] = new PercentModifiedValue(0, 100);
                PercentModifiedValue value = unit.EvaluateStat(stat);
                _values[(unit.Id, stat)] = value;

                return value;
            }
        }
    }
}
