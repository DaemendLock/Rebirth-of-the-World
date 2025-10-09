namespace Combat.API.DTO
{
    public readonly ref struct ScriptedSkillContext
    {
        public readonly SceneApi Enviroment;
        public readonly SkillApi Skill;
        public readonly Unit Owner;

        public ScriptedSkillContext(SkillApi skill, Unit owner, SceneApi environment)
        {
            Skill = skill;
            Owner = owner;
            Enviroment = environment;
        }
    }
}
