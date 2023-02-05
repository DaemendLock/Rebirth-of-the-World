using System.Collections.Generic;

public abstract class Charge : UnitTargetAbility {

    //ID: 24
    public Charge(Unit owner) : base(owner) {
    }

    public override void OnSpellStart() {
        Owner.AddNewStatus(CursorTarget, this, "charging", new Dictionary<string, float> { ["duration"] = 3});
    }
}

public class Charging : Status {
    public Charging(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    public override void OnCreated() {
        
    }

    

    public override void OnRemoved() {
        
    }

    private void Reach() {
        Parent.GiveMana(20, 1);
        Parent.SetTarget(Caster);
        Remove();
    }
}