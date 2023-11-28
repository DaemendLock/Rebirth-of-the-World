using UnityEngine;
using Utils.DataStructure;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class StatsWidget : MonoBehaviour
    {
        private StatsTable _stats = StatsTable.UNIT_DEFAULT;

        public void ShowStats(StatsTable stats)
        {
            _stats = stats;
        }
    }
}
