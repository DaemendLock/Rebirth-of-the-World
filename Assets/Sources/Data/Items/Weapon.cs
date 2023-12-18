using UnityEngine;

namespace Data.Items
{
    public enum WeaponType : byte
    {
        Sword,
        Dagger,
        Fist,
        CombatShield,
        GreateSword,
        Lance,
        Staff,
        Shield,
        Catalist,
    }

    [CreateAssetMenu(menuName = "Assets/Gear/Weapon")]
    public class Weapon : Gear
    {
        [SerializeField] private WeaponType _type;

        public Weapon() : base(GearType.Weapon)
        {
        }

        public WeaponType WeaponType => _type;
    }
}
