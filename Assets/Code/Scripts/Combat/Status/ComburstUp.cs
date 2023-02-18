using System.Collections.Generic;
/*
public class ComburstUp : Status {
    public ComburstUp(Unit owner, Unit caster, Ability ability, Dictionary<string, float> data) : base(owner, caster, ability, data) {

    }

    private float manacost;

    public override void OnCreated() {
        manacost = Ability.ManaCost * 0.25f;
        StartIntervalThink(0.25f);
    }

    public override void OnIntervalThink() {
        if (Parent.GetResource((int) AbilityResource.RESOURCE_RIGHT) >= manacost) {
            Parent.SpendMana(manacost, (int) AbilityResource.RESOURCE_RIGHT);
        } else {
            Remove();
        }

    }

    public override float GetModifierHasteBonus_Percent() {
        return -30;
    }


}*/