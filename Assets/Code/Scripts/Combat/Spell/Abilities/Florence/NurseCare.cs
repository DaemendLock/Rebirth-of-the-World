using Combat.SpellOld;
using Combat.Status;
using Data;
using System.Collections.Generic;

public class NurseCare : NotargetAbility
{

    public override AbilityData AbilityData => RotW.GetAbilityDataById(1);

    public override float AbilityCooldown => 30;

    public override float CastTime => 0;

    public override float ChannelTime => 5;

    public override AbilityBehavior AbilityBehavior => AbilityBehavior.NO_TARGET;

    public override AbilityResource AbilityResource => AbilityResource.LEFT;

    public override float AbilityCost => 15;

    public override float AbilityDamage => 200;

    public float GetCastRadius() => 40;

    public override ushort SpellId => 1;

    private HealEvent _healEvent;

    static NurseCare()
    {
        RotW.LinkStatus("status_nursing", typeof(Nursing));
    }

    public NurseCare(Unit owner) : base(owner)
    {
        _healEvent = new HealEvent(null, owner, 0, this);
    }

    private float timeTotal = 0.5f;
    private float timeLeft = 0;

    public override void OnChannelStart()
    {
        timeLeft = 0;
    }

    public override void OnChannelThink(float time)
    {
        if (time >= timeLeft)
        {
            Tick();
        }
    }

    private void Tick()
    {
        timeLeft += timeTotal;
        if (Owner.GetResource((int) AbilityResource.RIGHT) < AbilityCost)
        {
            Owner.Interrupt();
            return;
        }
        Owner.SpendMana(AbilityCost, 1);

        List<Unit> allies = RotW.FindUnitsInRadius(Owner.Origin, 40, Owner, UnitTargetTeam.FRIENDLY, false);
        if (allies.Count == 0)
        {
            return;
        }
        Unit low = allies[0];
        foreach (Unit unit in allies)
        {
            if (unit.HealthPercent < low.HealthPercent)
                low = unit;
        }
        _healEvent.Target = low;
        _healEvent.Healing = Owner.Spellpower * AbilityDamage * 0.01f;
        RotW.ApplyHealing(_healEvent);

        low.AddNewStatus(Owner, this, "status_nursing", new Dictionary<string, float> { ["additional_stacks"] = 1 });
    }
}

public class Nursing : Status
{

    private static readonly int STACK_DAMAGE_REDUCTION = 16;

    public override OldStatsTable Bonuses => OldStatsTable.EMPTY_TABLE;

    public Nursing(Unit owner, Unit caster, OldAbility ability, Dictionary<string, float> data) : base(owner, caster, ability, data, (long) modifierfunction.MODIFIER_PROPERTY_DAMAGE_RECIVE_PERCENT)
    {
    }

    public override void OnCreated(Dictionary<string, float> param)
    {
        StackCount += param["additional_stacks"];
        if (StackCount > 5)
            StackCount = 5;
    }

    public override void OnRefresh(Dictionary<string, float> param)
    {
        StackCount += param["additional_stacks"];
        if (StackCount > 5)
            StackCount = 5;
    }

    public override float GetDamageRecivePercentBonus(AttackEventInstance e)
    {
        float res = -STACK_DAMAGE_REDUCTION * StackCount;
        StackCount--;

        if (StackCount == 0)
            Remove();
        return res;
    }
}