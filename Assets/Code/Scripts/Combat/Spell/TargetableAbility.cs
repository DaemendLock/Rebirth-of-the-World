namespace Combat.SpellOld
{
    public abstract class TargetableAbility : OldAbility, ITargetable
    {

        public TargetableAbility(Unit owner) : base(owner)
        {

        }

        public abstract float CastRadius { get; }

        public override bool ShowOnTooltip => true;
    }
}