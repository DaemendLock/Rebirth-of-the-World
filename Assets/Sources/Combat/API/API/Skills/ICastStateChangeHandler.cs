namespace Combat.API.Skills
{
    public interface ICastStateChangeHandler : ISkillProperty
    {
        void OnStartup() { }
        void OnActive() { }
        void OnGapStart() { }
        void OnRecovery() { }
        void OnEnds() { }
    }
}
