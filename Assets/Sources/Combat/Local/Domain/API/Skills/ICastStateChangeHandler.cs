using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.Skills
{
    public interface IPassiveSkill
    {
        StatusName PassiveStatusName { get; }
    }

    public interface ICastStateChangeHandler
    {
        void OnStartup() { }
        void OnActive() { }
        void OnGapStart() { }
        void OnRecovery() { }
        void OnEnds() { }
    }
}
