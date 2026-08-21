using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("Paladin1")]
    public class Paladin1 : SkillScript, ICastableSkill
    {
        public bool OnCast()
        {
            Scene.CreateStatus(new(Owner.Id, "Paladin1Aura", 1f, 1, Instance.AbilityKey));
            return false;
        }
    }

    public sealed class Paladin_1New : ICastableNew
    {
        public bool OnCast(ICastContext actor, ISkillContext skillContext)
        {
            skillContext.GetCapability<IEncounterContext>().CreateStatus(new(actor.Caster.Id, "Paladin1Aura", 1f, 1, skillContext.Key));
            return false;
        }
    }
}
