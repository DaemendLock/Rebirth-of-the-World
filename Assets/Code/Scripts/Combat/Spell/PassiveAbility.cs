namespace Combat.SpellOld
{
    public abstract class PassiveAbility : OldAbility
    {
        protected PassiveAbility(Unit owner) : base(owner)
        {
        }

        public override bool ShowOnTooltip => false;
    }
}

