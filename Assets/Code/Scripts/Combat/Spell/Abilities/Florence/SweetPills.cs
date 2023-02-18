using Data;
using System.Collections.Generic;

public class SweetPills : NotargetAbility {
    public override AbilityData AbilityData => RotW.GetAbilityDataById(5);

    public override float AbilityCooldown => 20;

    public override float CastTime => 0;

    public override float ChannelTime => 0;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.NO_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.LEFT;

    public override float AbilityCost => 100;

    public override float AbilityDamage => 100;

    public override ushort SpellId => 5;

    //ID: 5
    public SweetPills(Unit owner) : base(owner) {

    }



    public override void OnSpellStart() {
        Owner.GiveMana(AbilityDamage, 1);
    }
}

public class SweetDream : Status {
    public SweetDream(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    private float healAmmount;

    public override StatsTable Bonuses => StatsTable.EMPTY_TABLE;

    public override void OnCreated() {

    }
}