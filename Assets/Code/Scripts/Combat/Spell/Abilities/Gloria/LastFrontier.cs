using Data;
using System.Collections.Generic;

public abstract class LastFronier : NotargetAbility {




    private static readonly AbilityData abilityData = RotW.GetAbilityDataById(23);


    static LastFronier() {
        RotW.LinkStatus("status_lastfronier", typeof(LastFronierStatus));
    }
    public LastFronier(Unit owner) : base(owner) {
    }

    public override void OnSpellStart() {
        Owner.AddNewStatus(Owner, this, "status_lastfronier", new Dictionary<string, float> { ["duration"] = 15 });
    }
}

public class LastFronierStatus : Status {
    public LastFronierStatus(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    modifierfunction[] func = { modifierfunction.MODIFIER_PROPERTY_RECEIVE_DAMAGE_PERCENT };
    public override modifierfunction[] DeclareFunctions() {
        return func;
    }

    public override float GetModifierReceiveDamage_Percent(AttackEventInstance e) {
        return -50;
    }
}