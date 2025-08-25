using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.DTO
{
    public readonly ref struct StatusApplicationData
    {

    }

    public readonly struct SkillScriptContext
    {
        public readonly Skill Skill;
        public readonly Unit Caster;

        public SkillScriptContext(Unit actor, Skill skill)
        {
            Skill = skill;
            Caster = actor;
        }
    }
}
