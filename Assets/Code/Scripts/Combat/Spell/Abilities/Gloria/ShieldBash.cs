using Combat.SpellOld;

public abstract class ShieldBash : UnitTargetAbility {
    //ID: 21
    public ShieldBash(Unit owner) : base(owner) {
        damageEvent = new DamageEvent(null, owner, 0, this, DamageFlags.NONE);
    }

    DamageEvent damageEvent;

    public override void OnSpellStart() {
        damageEvent.Victim = CursorTarget;
        damageEvent.Damage = Owner.Attack * AbilityDamage * 0.01f;
        RotW.ApplyDamage(damageEvent);
    }
}
