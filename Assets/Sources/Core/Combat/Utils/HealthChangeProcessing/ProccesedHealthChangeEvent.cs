using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Utils.HealthChangeProcessing
{
    public class ProccesedHealthChangeEvent
    {
        private PercentModifiedValue _value;
        private readonly List<HealthChangePrecessingData> _actions;

        public ProccesedHealthChangeEvent(HealthChangeEventData source)
        {

        }

        public void ApplyModification(HealthChangePrecessingData modification)
        {
            _actions.Add(modification);

            switch (modification.Action)
            {
                case HealthChangeProcessingType.INITIAL_VALUE:
                    _value.BaseValue = modification.Value;
                    break;

                case HealthChangeProcessingType.CONSTANT_MODIFICATION:
                    _value.BaseValue += modification.Value;
                    break;

                case HealthChangeProcessingType.PERCENT_MODIFICATION:
                    _value.Percent += modification.Value;
                    break;

                case HealthChangeProcessingType.EVADE:
                    _value.BaseValue = 0;
                    break;
            }
        }
    }
}
