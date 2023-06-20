using Combat.SpellOld;
using Combat.Status;
using Data;
using System.Collections.Generic;

public class LastFronier : NotargetAbility {

    private static readonly AbilityData abilityData = RotW.GetAbilityDataById(23);

    static LastFronier() {
        RotW.LinkStatus("status_lastfronier", typeof(LastFronierStatus));
    }
    public LastFronier(Unit owner) : base(owner) {
    }

    public override AbilityData AbilityData => throw new System.NotImplementedException();

    public override float AbilityCooldown => throw new System.NotImplementedException();

    public override float CastTime => throw new System.NotImplementedException();

    public override float ChannelTime => throw new System.NotImplementedException();

    public override AbilityBehavior AbilityBehavior => throw new System.NotImplementedException();

    public override AbilityResource AbilityResource => throw new System.NotImplementedException();

    public override float AbilityCost => throw new System.NotImplementedException();

    public override float AbilityDamage => throw new System.NotImplementedException();

    public override ushort SpellId => throw new System.NotImplementedException();

    public override void OnSpellStart() {
        Owner.AddNewStatus(Owner, this, "status_lastfronier", new Dictionary<string, float> { ["duration"] = 15 });
    }
}

public class LastFronierStatus : Status {
    public LastFronierStatus(Unit owner, Unit caster, OldAbility ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    
    }

    public override OldStatsTable Bonuses => OldStatsTable.EMPTY_TABLE;

    public override float GetDamageRecivePercentBonus(AttackEventInstance e) {
        return -50;
    }
}