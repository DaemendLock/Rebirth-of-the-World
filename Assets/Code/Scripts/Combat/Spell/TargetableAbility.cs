using Abilities;

public abstract class TargetableAbility : Ability, ITargetable {

    public TargetableAbility(Unit owner) : base(owner) {

    }

    public abstract float CastRadius { get; }

    public override bool ShowOnTooltip => true;
}
