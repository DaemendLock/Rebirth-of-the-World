public  abstract class ShieldBash : UnitTargetAbility {

    //ID: 21
    public ShieldBash(Unit owner) : base(owner) {
        dmgEvent = new DamageEvent(null, owner, 0, this, DamageFlags.NONE);
    }

    DamageEvent dmgEvent;

    public override void OnSpellStart() {
        dmgEvent.victim = CursorTarget;
        dmgEvent.damage = Owner.Attack * AbilityDamage * 0.01f;
        RotW.ApplyDamage(dmgEvent);
    }
}
