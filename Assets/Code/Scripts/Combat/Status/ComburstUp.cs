using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

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
            Parent.SpendMana(manacost, (int)AbilityResource.RESOURCE_RIGHT);
        } else {
            Remove();
        }
            
    }

    private readonly modifierfunction[] functions = new modifierfunction[2] {
        modifierfunction.MODIFIER_PROPERTY_HASTE_BONUS,
        modifierfunction.MODIFIER_PROPERTY_HASTE_BONUS_PERCENT
    };

    public override modifierfunction[] DeclareFunctions() {
        return functions;
    }

    public override float GetModifierHasteBonus_Percent() {
        return -30;
    }


}