public abstract class Swing : UnitTargetAbility {

    DamageEvent dmgEvent;

    //ID: 20
    public Swing(Unit owner) : base(owner) {
        dmgEvent = new DamageEvent(null, owner, 0, this, 0);
    }

    public override void OnSpellStart() {
        dmgEvent.Victim = CursorTarget;
        dmgEvent.Damage = Owner.Attack * AbilityDamage * 0.01f;
        RotW.ApplyDamage(dmgEvent);
        Owner.GiveMana(dmgEvent.Damage * 2, 1);
    }
}
