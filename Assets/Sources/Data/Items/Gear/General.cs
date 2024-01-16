using UnityEngine;

namespace Data.Items
{
    public enum ArmorClass
    {
        Fabric, Leather, Chainmail, Plate
    }

    public abstract class GeneralArmor : Gear
    {
        [SerializeField] private ArmorClass _class;

        protected GeneralArmor(GearType type) : base(type)
        {
        }
    }

    [CreateAssetMenu(menuName = "Assets/Gear/Head")]
    public class Head : GeneralArmor
    {
        //Pref Crit, Haste, SP, Lich, Parry, Evade
        public Head() : base(GearType.Head)
        {
        }
    }

    [CreateAssetMenu(menuName = "Assets/Gear/Body")]
    public class Body : GeneralArmor
    {
        //Pref HP, ATK, SP, Verse, Haste, AoeRes, (+ Block, Parry, Evade)
        public Body() : base(GearType.Body)
        {
        }
    }

    [CreateAssetMenu(menuName = "Assets/Gear/Legs")]
    public class Legs : GeneralArmor
    {
        //Pref HP, ATK, Verse, Crit, MoveSpeed, (+ Block, Evade )
        public Legs() : base(GearType.Legs)
        {
        }
    }
}
