namespace Combat.SpellOld
{
    public abstract class NotargetAbility : OldAbility
    {

        public override bool ShowOnTooltip => true;

        public NotargetAbility(Unit owner) : base(owner)
        {
        }

        public override UnitFilterResult StartAbility()
        {
            OnSpellStart();
            if (Castable == false)
            {
                PayManaCost();
                StartCooldown();
            }
            return UnitFilterResult.UF_SUCCESS;
        }
    }
}