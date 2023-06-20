

using Data;
using UnityEngine;

public class Gloria : Unit {
    public const ushort UNIT_ID = 2;

    private static readonly OldUnitData _baseUnit = new(UNIT_ID) {
        Role = UNIT_ROLE.TANK,
        Attack = new Stat() { baseValue = 100, gain = new float[1] { 0 } },
        Spellpower = new Stat() { baseValue = 50, gain = new float[1] { 0 } },
        Crit = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Versa = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        MaxHp = new Stat() { baseValue = 3000, gain = new float[1] { 0 } },
        MaxResource = new float[] { 100f, 100f },
        ResourceRegen = new float[] { 0, 0 },
        Lifesteal = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Movespeed = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        AoeResist = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Block = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Parry = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Evade = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        RindRadius = 0.5f,
        AttackSpeed = 1f,
        RangedAttack = false,
        ProjectileSpeed = 0,
        GalleryCardName = "GloriaCard",
        UnitPrefabName = "Gloria"
    };

    static Gloria() {
        UnityEngine.Debug.Log("Gloria loaded");
    }
    public Gloria(Vector3 location, Quaternion facing, Team team, byte lvl, byte rank, int objectId) : base(location, facing, team, lvl, rank, objectId){
    }

    protected override OldUnitData BaseUnit => _baseUnit;

    protected override void Precache() {
        //RotW.Precache("RefreshingConcoctionIcon", "Sprites/Abilities/Florence/SpellRefreshingConcoction", ResourceType.SPRITE);
    }

    public override void Init() {
        Init(
           //attackSpeed: 1.3f, moveSpeed: 5f,
           // turnRate: 2f, baseHp: 3000,
           // baseResource: new float[2] { 100, 1000 },
           //  baseResourceRegen: new float[2] { 0, 0 },
           //  attackRange: 1
           );
        LeftColor = Color.red;
        RightColor = Color.blue;
        SetResource(0, 0);
        SetResource(0, 1);
    }
}