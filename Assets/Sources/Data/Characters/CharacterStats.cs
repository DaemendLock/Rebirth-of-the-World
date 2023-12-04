using System;
using UnityEngine;
using Utils.DataStructure;

namespace Data.Characters
{
    [Serializable]
    public class CharacterStats
    {
        [SerializeField] private StatsTable _default = StatsTable.UNIT_DEFAULT;
        [SerializeField] private StatsTable _growth;

        public StatsTable GetStatsForLevel(int level)
        {
            StatsTable result = new StatsTable();

            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                result[i] = _default[i] + (_growth[i] * level);
            }

            return result;
        }
    }
}
