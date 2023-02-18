public abstract class PassiveAbility : Ability {
    protected PassiveAbility(Unit owner) : base(owner) {
    }

    public override bool ShowOnTooltip => false;
}

