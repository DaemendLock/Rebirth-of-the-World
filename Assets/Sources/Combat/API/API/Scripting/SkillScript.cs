namespace Combat.API.Scripting
{
    public class SkillScript : ISkillProperty
    {
        protected SkillApi Instance { get; private set; }

        public void Init(SkillApi instance)
        {
            Instance = instance;
            OnInit();
        }

        protected virtual void OnInit() { }
    }
}
