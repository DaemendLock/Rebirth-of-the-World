using Combat.SpellOld;
using Combat.Status;
using System.Collections.Generic;

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
    public MasterBlockShieldRegen(Unit owner, Unit caster, OldAbility ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {
    }

    public override OldStatsTable Bonuses => OldStatsTable.EMPTY_TABLE;

    public override void OnCreated() {
        StartIntervalThink(1);
    }

    public override void OnIntervalThink() {
        Caster.GiveMana(Caster.Attack, 1);
    }
}

public class BlockingShield : OvershieldStatus {

    private const float DamageConverationPercent = 0.3f;

    public BlockingShield(Unit owner, Unit caster, OldAbility ability, Dictionary<string, float> data) : base(owner, caster, ability, data, 0, false) {
        owner.RecivedDamage += OnDamage;
        DurabilityUpdated += ManageResource;
    }

    public override OldStatsTable Bonuses => OldStatsTable.EMPTY_TABLE;

    public void OnDamage(AttackEventInstance e) {
        Parent.GiveMana(e.OriginalValue * DamageConverationPercent, 0);
    }

    private void ManageResource(float value) {
        Parent.SetResource(Durability, 1);
    }
    
    public override void OnDestroy() {
        Parent.RecivedDamage -= OnDamage;
        DurabilityUpdated -= ManageResource;
    }
}