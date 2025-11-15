using Combat.API.Scripting;
using Combat.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("Paladin1")]
    public class Paladin1 : SkillScript, ICastableSkill
    {
        public void OnCast()
        {
            Owner.ApplyStatus(new("Paladin1Aura", 5f, 1, Skill));
            //Owner.GiveResource(new(new(2), 1, Skill));
        }
    }

    [SkillScriptName("DpsPaladin3")]
    public class DpsPaladin3 : SkillScript, ICastableSkill
    {
        public void OnCast()
        {
            float energy = Owner.GetResourceValue(new(2));

            //DealAoeDamage(energy * spellPower);
            Owner.ApplyStatus(new("DpsPaladin3Buff", 1f, 1, Skill));
            Owner.SpendResource(new(2), energy, Skill);
        }
    }
}
