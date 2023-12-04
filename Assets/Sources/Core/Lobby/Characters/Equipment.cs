using Core.Combat.Auras.AuraEffects;
using Data.Items;
using UnityEditor.Graphs;
using Utils.DataStructure;

namespace Core.Lobby.Characters
{
    public enum GearSlot : byte
    {
        Head,
        Body,
        Legs,
        MainHand,
        OffHand,
        Ring1,
        Ring2,
        Consumable1,
        Consumable2,
    }

    public class Equipment
    {
        private const byte GearSlotCount = 9;

        private Gear[] _equipment = new Gear[GearSlotCount];

        public void Equip(Gear gear, GearSlot slot)
        {
            _equipment[(int) slot] = gear;
        }

        public StatsTable GetStats()
        {
            StatsTable result = new();

            foreach (Gear gear in _equipment)
            {
                if(gear == null)
                {
                    continue;
                }

                result += gear.Stats;
            }

            return result;
        }
    }
}
