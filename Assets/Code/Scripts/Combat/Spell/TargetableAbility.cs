using Abilities;
using Data;

public abstract class TargetableAbility : Ability, ITargetable {

    public TargetableAbility(Unit owner) : base(owner) {
        
    }

    public abstract float CastRadius { get; }
}
