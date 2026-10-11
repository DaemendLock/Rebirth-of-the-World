using Combat.Common.ValueObjects;

namespace Combat.API.Skills
{
    public interface ICastStateChangeHandler : ISkillProperty
    {
        void OnStartup() { }
        void OnActive() { }
        void OnGapStart() { }
        void OnRecovery() { }
        void OnEnds() { }

        void OnInterrupt(InterruptReason reason) { }
    }
}
