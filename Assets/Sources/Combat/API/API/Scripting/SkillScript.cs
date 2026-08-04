namespace Combat.API.Scripting
{
    public class SkillScript : ISkillProperty
    {
        protected IAbilityApi Instance { get; private set; }

        protected IEncounterApi Scene => Instance.Scene;

        protected IUnit Owner => Instance.Owner;

        public void Init(IAbilityApi instance)
        {
            Instance = instance;
            OnInit();
        }

        protected virtual void OnInit() { }
    }
}
