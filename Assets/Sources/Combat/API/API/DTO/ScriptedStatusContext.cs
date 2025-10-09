namespace Combat.API.DTO
{
    public readonly ref struct ScriptedStatusContext
    {
        public ScriptedStatusContext(StatusApi instance, Unit parent, SkillApi source, SceneApi scene)
        {
            Parent = parent;
            Source = source;
            Instacne = instance;
            Scene = scene;
        }

        public StatusApi Instacne { get; }

        public SceneApi Scene { get; }

        public Unit Parent { get; }

        public SkillApi Source { get; }
    }
}
