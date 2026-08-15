using Combat.API.Scripting;
using Combat.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("Paladin1")]
    public class Paladin1 : SkillScript, ICastableSkill
    {
        public bool OnCast()
        {
            Scene.CreateStatus(new(Owner, "Paladin1Aura", 1f, 1, Instance));
            return false;
        }
    }
}
