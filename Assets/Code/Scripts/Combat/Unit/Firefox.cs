
using Events;
using UnityEngine;

public class Firefox : Unit {

    public override void Init() {
    }

    public override void OnDamage(AttackEventInstance e) {
        if (e.target == e.attacker && !HasStatus("comburst_status") && e.damage > 0)
            GiveMana(e.damage * 0.005f, (int) AbilityResource.RESOURCE_RIGHT);
    }

}