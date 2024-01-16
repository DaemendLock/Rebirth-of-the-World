using UnityEngine;

namespace Data.Items
{
    [CreateAssetMenu(menuName = "Assets/Gear/Ring")]
    public class Ring : GeneralArmor
    {
        //Pref ATK, SP, Haste, Verse, Crit, MoveSpeed, AoeRes, Lich, Evade, Parry, Block (Any/HP)
        public Ring() : base(GearType.Ring)
        {
        }
    }
}
