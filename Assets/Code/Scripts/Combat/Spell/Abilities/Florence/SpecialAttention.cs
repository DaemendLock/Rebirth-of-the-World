using Data;
using System.Collections.Generic;

public sealed class SpecialAttention : UnitTargetAbility {

    private static readonly AbilityData _abilityData = RotW.GetAbilityDataById(3);

    public override AbilityData AbilityData => _abilityData;

    public override float AbilityCooldown => 20;

    public override float CastTime => 3;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.UNIT_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.RESOURCE_LEFT;

    public override float AbilityCost => 100;

    public override float AbilityDamage => 200;

    public override ushort SpellId => 3;

    public override float ChannelTime => 0;

    public override UNIT_TARGET_FLAGS TargetFlag => UNIT_TARGET_FLAGS.NONE;

    public override UNIT_TARGET_TEAM TargetTeam => UNIT_TARGET_TEAM.FRIENDLY;

    public override float CastRadius => 40;

    //ID: 3
    public SpecialAttention(Unit owner) : base(owner) {
    }

    

    public override void OnCastFinished(bool succes) {
        if (!succes ) return;
        Owner.SpendMana(20, 1);
        UnityEngine.Debug.Log(Owner.Spellpower * AbilityDamage * 0.01f * (2 - CursorTarget.HealthPercent));
        CursorTarget.Heal(Owner.Spellpower * AbilityDamage * 0.01f * ( 2 - CursorTarget.HealthPercent), this);
        
        CursorTarget.AddNewStatus(Owner, this, "status_nursing", new Dictionary<string, float>() { ["additional_stacks"] = 5});
    }
}