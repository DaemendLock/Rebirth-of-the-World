

using UnityEngine;

public class Gloria : Unit {
    protected override void Precache() {
        //RotW.Precache("RefreshingConcoctionIcon", "Sprites/Abilities/Florence/SpellRefreshingConcoction", ResourceType.SPRITE);
    }

    public override void Init() {
        Init(
            //attackSpeed: 1.3f, moveSpeed: 5f,
            // turnRate: 2f, baseHp: 3000,
            // baseResource: new float[2] { 100, 1000 },
           //  baseResourceRegen: new float[2] { 0, 0 },
           //  attackRange: 1
           );
        leftColor = Color.red;
        rightColor = Color.blue;
        SetResource(0, 0);
        SetResource(0, 1);
    }
}