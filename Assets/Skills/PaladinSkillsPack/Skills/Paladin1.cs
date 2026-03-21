using Combat.API.Scripting;
using Combat.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("Paladin1")]
    public class Paladin1 : SkillScript, ICastableSkill
    {
        public void OnCast(CastEvent @event)
        {
            @event.Scene.CreateStatus(new(@event.Caster, "Paladin1Aura", 1f, 1, @event.Skill));
            //Owner.GiveResource(new(new(2), 1, Skill));
        }
    }

    [SkillScriptName("DpsPaladin3")]
    public class DpsPaladin3 : SkillScript, ICastableSkill
    {
        public void OnCast(CastEvent @event)
        {
            float energy = @event.Caster.GetResourceValue(new(2));

            //DealAoeDamage(energy * spellPower);
            @event.Scene.CreateStatus(new(@event.Caster, "DpsPaladin3Buff", 1f, 1, @event.Skill));
            @event.Caster.SpendResource(new(2), energy, @event.Skill);
        }
    }
}
