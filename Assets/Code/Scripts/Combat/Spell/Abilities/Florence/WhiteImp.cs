using Data;
using System.Collections.Generic;

public class WhiteImp : NotargetAbility {
    public override AbilityData AbilityData => RotW.GetAbilityDataById(4);

    public override float AbilityCooldown => 15;

    public override float CastTime => 0;

    public override float ChannelTime => 0;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.NO_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.RESOURCE_RIGHT;

    public override float AbilityCost => 0;

    public override float AbilityDamage => 0;

    public override ushort SpellId => 4;

    static WhiteImp() {
        RotW.LinkStatus("status_whitedevil", typeof(Whitedevil));
    }

    //ID:4
    public WhiteImp(Unit owner) : base(owner) {
    }

    public override void OnSpellStart() {
        Owner.SetResource(0, 1);
        Owner.GiveMana(100, 0);
    }

    public override string GetPassiveStatusName() {
        return "status_whitedevil";
    }
}

public class Whitedevil : Status {
    public Whitedevil(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    public override void OnCreated() {
        StartIntervalThink(0.5f);
    }

    public override void OnIntervalThink() {
        if (Parent.GetResourcePercent(1) >= 1) {
            Parent.SpendMana(5,0);
        }
        else Parent.GiveMana(5, 0);
    }


}
