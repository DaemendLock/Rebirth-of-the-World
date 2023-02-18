
using Data;
using System.Collections.Generic;

public class RefreshingConcoction : UnitTargetAbility {

    public override AbilityData AbilityData => RotW.GetAbilityDataById(0);

    public override float AbilityCooldown => 30;

    public override float CastTime => 0;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.UNIT_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.RIGHT;

    public override float AbilityCost => 0;

    public override float AbilityDamage => 0.4f;

    public override ushort SpellId => 0;

    public override float ChannelTime => 0;

    public override UNIT_TARGET_FLAGS TargetFlag => UNIT_TARGET_FLAGS.NONE;

    public override UNIT_TARGET_TEAM TargetTeam => UNIT_TARGET_TEAM.FRIENDLY;

    public override float CastRadius => 60;

    static RefreshingConcoction() {
        RotW.LinkStatus("status_trepidation", typeof(Trepidation));
    }

    //ID: 0
    public RefreshingConcoction(Unit owner) : base(owner) {
    }

    public override void OnSpellStart() {
        Owner.SetResource(0, 1);
        CursorTarget.SetHealth(CursorTarget.CurrentHealth * (1 - AbilityDamage));
        CursorTarget.AddNewStatus(Owner, this, "status_trepidation", new Dictionary<string, float> { ["duration"] = 7 });
    }


}

public class Trepidation : Status {
    public Trepidation(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    public override StatsTable Bonuses => new() { AtkPercent = 60, SpellpowerPercent = 60 };
}
