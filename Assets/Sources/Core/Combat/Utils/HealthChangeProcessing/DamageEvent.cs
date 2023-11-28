using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Utils.HealthChangeProcessing
{
    [Flags]
    public enum DamageEventFlags
    {
        NONE = 0,
        CANT_EVADE = 1,
        CANT_PARRY = 2,
        CANT_BLOCK = 4,
        CRITICAL = 5,
    }

    public class DamageEvent
    {
        private HealthChangeEventData _source;

        private PercentModifiedValue _outcomeAmplification = new(0, 0);
        private PercentModifiedValue _incomeReduction = new(0, 0);
        private readonly List<HealthChangeEventModification> _actions = new(8);

        private float _absorbtion = 0;
        private float _versalityAmplification = 1;

        private DamageEventFlags _flags;

        public DamageEvent(HealthChangeEventData source)
        {
            _source = source;
        }

        public HealthChangeFailType Result { get; private set; } = HealthChangeFailType.NONE;

        public void Absorb(float value, int caster)
        {
            _absorbtion += value;

            RecordAction(new HealthChangeEventModification(caster, HealthChangeProcessingType.ABSORB, value));
        }

        public void ApplyIncomeReduction(PercentModifiedValue value, int caster)
        {
            _incomeReduction += value;

            if (value.BaseValue != 0)
            {
                RecordAction(new(caster, HealthChangeProcessingType.CONSTANT_MODIFICATION, value.BaseValue));
            }

            if (value.Percent != 0)
            {
                RecordAction(new(caster, HealthChangeProcessingType.PERCENT_MODIFICATION, value.Percent));
            }
        }

        public void ApplyOutcomeAmplification(PercentModifiedValue value, int caster)
        {
            _outcomeAmplification += value;

            if (value.BaseValue != 0)
            {
                RecordAction(new(caster, HealthChangeProcessingType.CONSTANT_MODIFICATION, value.BaseValue));
            }

            if (value.Percent != 0)
            {
                RecordAction(new(caster, HealthChangeProcessingType.PERCENT_MODIFICATION, value.Percent));
            }
        }

        public void ApplyVerasilityMultiplier(float value, int caster)
        {
            _versalityAmplification *= value;

            RecordAction(new HealthChangeEventModification(caster, HealthChangeProcessingType.AMPLIFY, value));
        }

        public void ApplyVerasilityDivider(float value, int caster)
        {
            _versalityAmplification /= value;
            RecordAction(new HealthChangeEventModification(caster, HealthChangeProcessingType.RESIST, value));

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Evade(int caster)
        {
            if (_flags.HasFlag(DamageEventFlags.CANT_EVADE) || _flags.HasFlag(DamageEventFlags.CRITICAL))
            {
                return;
            }

            RecordAction(new(caster, HealthChangeProcessingType.EVADE, 0));

            Result = HealthChangeFailType.EVADE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Parry(int caster)
        {
            if (_flags.HasFlag(DamageEventFlags.CANT_PARRY) || (Result == HealthChangeFailType.EVADE))
            {
                return;
            }

            RecordAction(new(caster, HealthChangeProcessingType.PARRY, 0));

            Result = HealthChangeFailType.PARRY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Block(int caster)
        {
            if (_flags.HasFlag(DamageEventFlags.CANT_BLOCK) || (Result == HealthChangeFailType.PARRY) || (Result == HealthChangeFailType.EVADE))
            {
                return;
            }

            RecordAction(new(caster, HealthChangeProcessingType.BLOCK, 0));

            Result = HealthChangeFailType.BLOCK;
        }

        public float EvaluateDamage()
        {
            if (Result == HealthChangeFailType.EVADE || Result == HealthChangeFailType.PARRY)
            {
                return 0;
            }

            float result = ((_source.Value + _outcomeAmplification.BaseValue)
                    * _outcomeAmplification.Percent * _versalityAmplification / 100
                    - _incomeReduction.BaseValue) * 100 / _incomeReduction.Percent;

            if (Result == HealthChangeFailType.BLOCK)
            {
                result /= 2;
            }

            return MathF.Max(result - _absorbtion, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RecordAction(HealthChangeEventModification action)
        {
            _actions.Add(action);
        }
    }
}
