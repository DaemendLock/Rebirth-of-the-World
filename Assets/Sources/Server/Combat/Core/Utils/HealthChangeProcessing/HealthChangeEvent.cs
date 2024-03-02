using Core.Combat.Units;
using Server.Combat.Core.Utils;
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
        CRITICAL = 5,
    }

    public enum HealthChangeProcessingType
    {
        INITIAL_VALUE,
        CONSTANT_MODIFICATION,
        PERCENT_MODIFICATION,
        ABSORB,
        RESIST,
        AMPLIFY,
    }

    public readonly struct HealthChangeEventModification
    {
        public readonly int Caster;
        public readonly HealthChangeProcessingType Action;
        public readonly float Value;

        public HealthChangeEventModification(int caster, HealthChangeProcessingType action, float value)
        {
            Caster = caster;
            Action = action;
            Value = value;
        }
    }

    public class HealthChangeEvent
    {
        private readonly HealthChangeEventData _source;

        private PercentModifiedValue _outcomeAmplification = new(0, 0);
        private PercentModifiedValue _incomeReduction = new(0, 0);
        private readonly List<HealthChangeEventModification> _actions = new(8);

        private float _absorbtion = 0;
        private float _versalityAmplification = 1;

        public HealthChangeEvent(HealthChangeEventData source)
        {
            _source = source;
        }

        public Unit Caster => _source.Caster;
        public Unit Target => _source.Target;
        public SpellId Source => _source.Spell;
        public HealthChangeFailType Result { get; private set; } = HealthChangeFailType.NONE;

        public void Absorb(float value, int caster)
        {
            _absorbtion += value;

            RecordAction(new HealthChangeEventModification(caster, HealthChangeProcessingType.ABSORB, value));
        }

        public void ApplyIncomeModification(PercentModifiedValue value, int caster)
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

        public void ApplyOutcomeModifiction(PercentModifiedValue value, int caster)
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

        public void ApplyVerasilityModification(float value, int casterId)
        {
            _versalityAmplification *= value;

            RecordAction(new HealthChangeEventModification(casterId, value > 1 ? HealthChangeProcessingType.AMPLIFY : HealthChangeProcessingType.RESIST, value));
        }

        public float Evaluate()
        {
            float result = ((_source.Value + _outcomeAmplification.BaseValue)
                * _outcomeAmplification.Percent * _versalityAmplification / 100
                    - _incomeReduction.BaseValue) * 100 / _incomeReduction.Percent;

            return MathF.Max(result - _absorbtion, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RecordAction(HealthChangeEventModification action)
        {
            _actions.Add(action);
        }
    }
}
