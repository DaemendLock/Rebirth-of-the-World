
public class Florence : Unit {

    protected override void Precache() {
        
        RotW.Precache("RefreshingConcoctionIcon", "Sprites/Abilities/Florence/SpellRefreshingConcoction", ResourceType.SPRITE);
        RotW.Precache("NurseCareIcon", "Sprites/Abilities/Florence/SpellNurseCare", ResourceType.SPRITE);
        RotW.Precache("SweetPillsIcon", "Sprites/Abilities/Florence/SpellSweetPills", ResourceType.SPRITE);
        RotW.Precache("SpecialAttentionIcon", "Sprites/Abilities/Florence/SpellSpecialAttention", ResourceType.SPRITE);
        RotW.Precache("RelifePleasureIcon", "Sprites/Abilities/Florence/SpellRelifePleasure", ResourceType.SPRITE);
    }
    

    public override void Init() {
        abilities[0] = new RelifePleasure(this);
        abilities[1] = new WhiteImp(this);
        abilities[2] = new SpecialAttention(this);
        abilities[3] = new NurseCare(this);
        abilities[4] = new SweetPills(this);
        abilities[5] = new RefreshingConcoction(this);;

    }
}