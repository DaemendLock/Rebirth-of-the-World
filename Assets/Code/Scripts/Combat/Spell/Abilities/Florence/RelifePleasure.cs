using Data;
using System.Collections.Generic;
using UnityEngine;

public sealed class RelifePleasure : UnitTargetAbility {
    public override AbilityData AbilityData => RotW.GetAbilityDataById(2);

    public override float AbilityCooldown => 0;

    public override float CastTime => 1.7f;

    public override float ChannelTime => 0;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.UNIT_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.RESOURCE_LEFT;

    public override float AbilityCost => 50;

    public override float AbilityDamage => 70;

    public override float CastRadius => 40;

    public override ushort SpellId => 2;

    public override UNIT_TARGET_FLAGS TargetFlag => UNIT_TARGET_FLAGS.NONE;

    public override UNIT_TARGET_TEAM TargetTeam => UNIT_TARGET_TEAM.FRIENDLY;

    static RelifePleasure(){
        RotW.LinkStatus("status_bluepotion", typeof(BluePotion));
        RotW.LinkStatus("status_redpotion", typeof(RedPotion));
        RotW.LinkStatus("status_purplepotion", typeof(PurplePotion));
    }

    //ID: 2
    public RelifePleasure(Unit owner) : base(owner) {
    }

    public override void OnCastFinished(bool succes) {
        if(!succes) return;
        Owner.SpendMana(10, 1);
        int cur = Random.Range(0, 3);
        if (cur == 0) {
            BlueEffect();
        } else if (cur == 1) {
            RedEffect();
        } else {
            PurpleEffect();
        }
    }

    private void BlueEffect() {
        
        CursorTarget.AddNewStatus(Owner, this, "status_bluepotion", new Dictionary<string, float> { ["duration"] = 5 });
        RotW.ApplyDamage(new DamageEvent(CursorTarget, Owner, Owner.Spellpower * 0.25f,this, DamageFlags.NON_LETHAL));
        
    }
    private void RedEffect() {
        CursorTarget.AddNewStatus(Owner, this, "status_redpotion",new Dictionary<string, float> { ["duration"] = 2.5f } );
    }

    private void PurpleEffect() {
        CursorTarget.AddNewStatus(Owner, this, "status_purplepotion", new Dictionary<string, float> { ["duration"] = 2f });
    }

    
}

public class BluePotion : Status {
    public BluePotion(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    modifierfunction[] func = { modifierfunction.MODIFIER_PROPERTY_HASTE_BONUS };
    public override modifierfunction[] DeclareFunctions() {
        return func;
    }

    public override float GetModifierHasteBonus() {
        return 20;
    }
}

public class RedPotion : Status {
    public RedPotion(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    private DamageEvent tick;

    public override void OnCreated(Dictionary<string, float> param) {
        tick = new DamageEvent(Parent, Caster, Caster.Spellpower*0.1f, Ability, DamageFlags.DOT_EFFECT | DamageFlags.NON_LETHAL);
        StartIntervalThink(param["duration"] * 0.2f);
    }

    public override void OnRefresh(Dictionary<string, float> param) {
        tick.damage = Caster.Spellpower * 0.1f;
        Parent.Heal(tick.damage * 10, Ability);
    }

    public override void OnIntervalThink() {
        RotW.ApplyDamage(tick);
    }

    public override void OnRemoved() {
        Parent.Heal(tick.damage *10, Ability);
    }
}

public class PurplePotion : Status {
    public PurplePotion(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    private float heal;

    public override void OnCreated(Dictionary<string, float> param) {
        heal = Caster.Spellpower * 0.1f;
        StartIntervalThink(param["duration"] * 0.1f);
    }

    public override void OnRefresh(Dictionary<string, float> param) {
        heal = Caster.Spellpower * 0.1f;
        Parent.Heal(heal, Ability);
    }

    public override void OnIntervalThink() {
        Parent.Heal(heal, Ability);
    }
}
