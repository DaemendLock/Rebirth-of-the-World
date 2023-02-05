using System.Collections.Generic;
using Unity.Mathematics;

public abstract class MasterBlock : NotargetAbility {

    static MasterBlock() {
        RotW.LinkStatus("status_blockingshield", typeof(BlockingShield));
        RotW.LinkStatus("status_masterblock_shield_regen", typeof(MasterBlockShieldRegen));
    }

    //ID: 22
    public MasterBlock(Unit owner) : base(owner) {
    }

    public override string GetPassiveStatusName() {
        return "status_blockingshield";
    }

    public override void OnSpellStart() {
        Owner.SetResource(Owner.GetResourceMax(1), 1);
        Owner.AddNewStatus(Owner, this, "status_masterblock_shield_regen", new Dictionary<string, float> { ["duration"] = 10 });
    }
}

public class MasterBlockShieldRegen : Status {
    public MasterBlockShieldRegen(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    public override void OnCreated() {
        StartIntervalThink(1);
    }

    public override void OnIntervalThink() {
        Caster.GiveMana(Caster.Attack, 1);
    }
}

public class BlockingShield : Status {

    private const float DamageConverationPercent = 0.3f;

    public BlockingShield(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    modifierfunction[] func = { modifierfunction.MODIFIER_EVENT_ON_DAMAGE, modifierfunction.MODIFIER_PROPERTY_DAMAGE_BLOCK };


    public override modifierfunction[] DeclareFunctions() {
        return func;
    }

    public override float GetModifierDamageBlock(AttackEventInstance e) {
        float mana = Parent.GetResource(1);
        Parent.SpendMana(e.damage, 1);
        return math.min(e.damage, mana);
    }

    public override void OnDamage(AttackEventInstance e) {
        Parent.GiveMana(e.damage * DamageConverationPercent, 0);
    }


}