using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("Paladin1")]
    public class Paladin1 : ScriptedSkill
    {
        public override void OnCast()
        {
            Caster.ApplyStatus(new("Paladin1Aura", this, 5f, 1));
        }
    }

    [SkillScriptName("DpsPaladin3")]
    public class DpsPaladin3 : ScriptedSkill
    {
        public override void OnCast()
        {
            float energy = Caster.GetResourceValue(new(2));

            //DealAoeDamage(energy * spellPower);
            Caster.ApplyStatus(new("DpsPaladin3Buff", this, 1f, 1));
            Caster.SpendResource(this, new(2), energy);
        }
    }
}
