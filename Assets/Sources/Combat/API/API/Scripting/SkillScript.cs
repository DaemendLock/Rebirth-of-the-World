namespace Combat.API.Scripting
{
    public class SkillScript : ISkillProperty
    {
        protected AbilityApi Instance { get; private set; }

        protected EncounterApi Scene => Instance.Scene;

        protected Unit Owner => Instance.Owner;

        public void Init(AbilityApi instance)
        {
            Instance = instance;
            OnInit();
        }

        protected virtual void OnInit() { }
    }
}
