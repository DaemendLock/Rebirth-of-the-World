public abstract class NotargetAbility : Ability
{
    public NotargetAbility(Unit owner) : base(owner) {
    }

    public override UnitFilterResult StartAbility() {
        OnSpellStart();
        if (Castable == false) { 
            PayManaCost();
            StartCooldown();
        }
        return UnitFilterResult.UF_SUCCESS;
    }
}
