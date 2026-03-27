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
        }
    }
}
