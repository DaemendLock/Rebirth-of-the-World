using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct ScriptedSkillContext
    {
        public readonly Scene Enviroment;
        public readonly Skill Skill;
        public readonly Unit Caster;

        public ScriptedSkillContext(Skill skill, Unit actor, Scene environment)
        {
            Skill = skill;
            Caster = actor;
            Enviroment = environment;
        }
    }
}
