using System;
using Utils.DataTypes;

namespace Core.Combat.Stats
{
    public enum UnitStat : int
    {
        ATK,
        SPELLPOWER,
        HASTE,
        CRIT,
        VERSALITY,
        LIFESTEAL,
        SPEED,
        AOERESIST,
        MAX_HEALTH,
        DAMAGE_DONE,
        DAMAGE_TAKEN,
        HEALING_DONE,
        HEALING_TAKEN,
        BLOCK,
        EVADE,
        PARRY,
    }

    [Serializable]
    public class StatsTable
    {
        public static readonly StatsTable UNIT_DEFAULT = new StatsTable(new PercentModifiedValue[]
        {
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
            new PercentModifiedValue(0, 100),
        });

        private PercentModifiedValue[] _values = new PercentModifiedValue[(int) UnitStat.PARRY];

        public StatsTable()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = new PercentModifiedValue();
            }
        }

        public StatsTable(PercentModifiedValue[] values)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
        }

        public void Add(StatsTable table)
        {
            for (UnitStat stat = UnitStat.ATK; stat <= UnitStat.PARRY; stat++)
            {
                _values[(int) stat] += table._values[(int) stat];
            }
        }

        public void Subtract(StatsTable table)
        {
            for (UnitStat stat = UnitStat.ATK; stat <= UnitStat.PARRY; stat++)
            {
                _values[(int) stat] -= table._values[(int) stat];
            }
        }

        public PercentModifiedValue this[UnitStat stat]
        {
            get => _values[(int) stat];
            set => _values[(int) stat] = value;
        }
    }
}
