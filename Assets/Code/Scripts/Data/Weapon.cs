using UnityEngine;

public enum WeaponType {
    SWORD,
    DAGGER,
    SPEAR,
    WAND,
    HANDGUN,
    SHIELD,
    GRIMOIRE,
    STAFF,
    CLAYMORE,
    BOW
}

[CreateAssetMenu(fileName = "Unnamed Weapon", menuName = "New Weapon", order = 57)]
public class Weapon : Gear {

    [SerializeField] private WeaponType _weaponType;

    public Weapon() : base(GearType.WEAPON) {
    }
}