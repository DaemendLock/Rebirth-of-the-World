using System;
using UnityEngine;

public enum UNIT_ROLE
{
    NONE,
    TANK,
    HEAL,
    DPS
}

namespace View.Lobby.CharacterSheet
{
    [Serializable]
    public class UnitPreview
    {
        //public OldUnitData baseData;

        //LEFT PART 
        [Header("Left part")] public Sprite Icon;

        [Header("Gear")] private UnitGear[] _gear = new UnitGear[9];

        public UnitGear[] GetGear()
        {
            /*Item item;
            foreach (int i in Gear)
            {
                if (i > 0 && Item.TryGetById(i, out item) && item.Type == Item.Category.GEAR)
                {
                    _gear[(int) ((OldGear) item).Slot] ??= new UnitGear((OldGear) item, ((OldGear) item).Slot);
                }
            }
            */

            return _gear;
        }

        public int[] Gear { get; private set; } = new int[9];

        //RIGHT PART
        [Header("Right part")]
        public string name;
        public float lvl;
        public float affection;
        public UNIT_ROLE role;
        //public List<AbilityData> abilities;
        //public OldStatsTable overallstats;

        public UnitPreview(/*OldUnitData baseData, Sprite icon, Dictionary<OldGearSlot, OldGear> gear, */float lvl, float affection)
        {
            //this.baseData = baseData;
            //this.Icon = Icon;
            /*if (gear != null)
                foreach (KeyValuePair<OldGearSlot, OldGear> kvp in gear)
                {
                    _gear[(int) kvp.Key] = new UnitGear(kvp.Value, kvp.Key);
                }
            name = baseData.Name;
            this.lvl = lvl;
            this.affection = affection;
            */
            //role = baseData.Role;
        }
    }
}