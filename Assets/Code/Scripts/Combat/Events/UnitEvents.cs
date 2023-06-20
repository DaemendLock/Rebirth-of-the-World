
using Combat.SpellOld;
using UnityEngine;

public enum DamageCategory {
    SPELL,
    ATTACK
}

public enum HealingFlags : int {
    NONE = 0,
    CANT_AMPLIFY = 1,
    HOT_EFFECT = 2,
}

public enum DamageFlags : int {
    NONE = 0,
    NON_LETHAL = 1,
    HPLOSS = 2,
    BYPASS_EVADE = 4,
    BYPASS_PARRY = 8,
    BYPASS_BLOCK = 16,
    DOT_EFFECT = 32
}

public enum attackfail : int {
    NONE,
    MISS,
    PARRY,
    BLOCK,
    INVULNERABLE,
    DEAD
}

public struct DamageEvent {
    public Unit Victim;
    public Unit Attacker;
    public float Damage;
    public OldAbility Ability;
    public DamageFlags DamageFlags;

    public DamageEvent(Unit victim, Unit attacker, float damage, OldAbility ability = null, DamageFlags damageFlags = 0) {
        Victim = victim;
        Attacker = attacker;
        Damage = damage;
        Ability = ability;
        DamageFlags = damageFlags;
    }
}

public struct HealEvent {
    public Unit Target;
    public Unit Healer;
    public float Healing;
    public OldAbility Ability;
    public HealingFlags HealingFlags;

    public HealEvent(Unit target, Unit healer, float healing, OldAbility ability, HealingFlags healingFlags = 0) {
        Target = target;
        Healer = healer;
        Healing = healing;
        Ability = ability;
        HealingFlags = healingFlags;
    }
}

public class HealthChangeEventInstance {
    public readonly Unit Inflictor;
    public readonly Unit Target;
    public readonly OldAbility Ability;
    public float Value;
    public readonly float OriginalValue;
    public attackfail FailType;

    public HealthChangeEventInstance(Unit inflicor, Unit target, OldAbility ability, float value, float originalValue) {
        Inflictor = inflicor;
        Target = target;
        Ability = ability;
        Value = value;
        OriginalValue = originalValue;
        FailType = attackfail.NONE;
    }
}

public class AttackEventInstance : HealthChangeEventInstance {
    public DamageCategory DamageCategory;
    public DamageFlags DamageFlags;

    public AttackEventInstance(Unit attacker, Unit target, OldAbility ability, float damage, float originalDamage, DamageCategory damageCategory, DamageFlags damageFlags) : base(attacker, target, ability, damage, originalDamage) {
        DamageCategory = damageCategory;
        DamageFlags = damageFlags;
    }
}

public class HealEventInstance : HealthChangeEventInstance {
    public HealingFlags HealingFlags;

    public HealEventInstance(Unit healer, Unit target, OldAbility ability, float healing, float originalHealing, HealingFlags healingFlags) : base(healer, target, ability, healing, originalHealing) {
        HealingFlags = healingFlags;
    }
}

public class UnitEventInstance {
    public Vector3 new_pos;
    public Unit unit;
    public UnitEventInstance(Vector3 new_pos, Unit unit) {
        this.unit = unit;
        this.new_pos = new_pos;
    }
}

public class AbilityEventInstance {
    private OldAbility _ability;
    public AbilityEventInstance(OldAbility ability) {
        _ability = ability;
    }

    public OldAbility Ability => _ability;
}
