using Data;
using UnityEngine;

public class Florence : Unit {

    public const ushort UNIT_ID = 1;

    private static readonly OldUnitData _baseUnit = new(UNIT_ID) {
        Role = UNIT_ROLE.HEAL,
        Attack = new Stat() { baseValue = 50, gain = new float[1] { 0 } },
        Spellpower = new Stat() { baseValue = 100, gain = new float[1] { 0 } },
        Crit = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Versa = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        MaxHp = new Stat() { baseValue = 1000, gain = new float[1] { 0 } },
        MaxResource = new float[] { 1000f, 100f },
        ResourceRegen = new float[] { 0, 10 },
        Lifesteal = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Movespeed = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        AoeResist = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Block = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Parry = new Stat() { baseValue = 0, gain = new float[1] { 0 } },
        Evade= new Stat() { baseValue = 0, gain=new float[1] { 0 } },
        RindRadius = 0.5f,
        AttackSpeed = 1.7f,
        RangedAttack = true,
        ProjectileSpeed = 10,
        GalleryCardName = "FlorenceCard",
        UnitPrefabName = "Florence"
    };

    public Florence(Vector3 location, Quaternion facing, Team team, byte lvl, byte rank, int objectId) : base(location, facing, team, lvl, rank, objectId) {
    }

    protected override OldUnitData BaseUnit => _baseUnit;

    protected override void Precache() {
        RotW.Precache("RefreshingConcoctionIcon", "Sprites/Abilities/Florence/SpellRefreshingConcoction", ResourceType.SPRITE);
        RotW.Precache("NurseCareIcon", "Sprites/Abilities/Florence/SpellNurseCare", ResourceType.SPRITE);
        RotW.Precache("SweetPillsIcon", "Sprites/Abilities/Florence/SpellSweetPills", ResourceType.SPRITE);
        RotW.Precache("SpecialAttentionIcon", "Sprites/Abilities/Florence/SpellSpecialAttention", ResourceType.SPRITE);
        RotW.Precache("RelifePleasureIcon", "Sprites/Abilities/Florence/SpellRelifePleasure", ResourceType.SPRITE);
    }

    public override void Init() {
        AddAbility(new RelifePleasure(this));
        AddAbility(new WhiteImp(this));
        AddAbility(new SpecialAttention(this));
        AddAbility(new NurseCare(this));
        AddAbility(new SweetPills(this));
        AddAbility(new RefreshingConcoction(this));
    }
}