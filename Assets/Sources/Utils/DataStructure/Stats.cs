using System;
using UnityEngine;
using Utils.DataTypes;

namespace Utils.DataStructure
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
        public const int STATS_COUNT = (int) UnitStat.PARRY + 1;

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
            new PercentModifiedValue(0, 100),
        });

        [SerializeField] private PercentModifiedValue[] _values = new PercentModifiedValue[STATS_COUNT];

        public StatsTable()
        {
            for (int i = 0; i < STATS_COUNT; i++)
            {
                _values[i] = new PercentModifiedValue();
            }
        }

        public StatsTable(PercentModifiedValue[] values)
        {
            for (int i = 0; i < STATS_COUNT; i++)
            {
                _values[i] = values[i];
            }
        }

        public void Add(StatsTable table)
        {
            for (int i = 0; i < STATS_COUNT; i++)
            {
                _values[i] += table._values[i];
            }
        }

        public void Subtract(StatsTable table)
        {
            for (int i = 0; i < STATS_COUNT; i++)
            {
                _values[i] -= table._values[i];
            }
        }

        public PercentModifiedValue this[int stat]
        {
            get => _values[stat];
            set => _values[stat] = value;
        }

        public PercentModifiedValue this[UnitStat stat]
        {
            get => _values[(int) stat];
            set => _values[(int) stat] = value;
        }

        public static StatsTable operator +(StatsTable value1, StatsTable value2)
        {
            PercentModifiedValue[] values = new PercentModifiedValue[STATS_COUNT];

            for (int i = 0; i < STATS_COUNT; i++)
            {
                values[i] = value1[i] + value2[i];
            }

            return new StatsTable(values);
        }
    }
}
