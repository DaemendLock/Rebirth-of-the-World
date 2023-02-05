using Events;
using System.Collections.Generic;

public abstract class Swing : UnitTargetAbility {

    DamageEvent dmgEvent;

    //ID: 20
    public Swing(Unit owner) : base(owner) {
        dmgEvent = new DamageEvent(null, owner, 0, this, 0);
    }

    public override void OnSpellStart() {
        dmgEvent.victim = CursorTarget;
        dmgEvent.damage = Owner.Attack * AbilityDamage * 0.01f;
        RotW.ApplyDamage(dmgEvent);
        Owner.GiveMana(dmgEvent.damage * 2, 1);
    }
}
