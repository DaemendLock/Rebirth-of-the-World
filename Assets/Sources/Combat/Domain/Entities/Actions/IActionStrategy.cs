using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Entities
{
    public interface IActionStrategy
    {
        bool IsComplete { get; }
        float EffectiveTime { get; }

        void Start();
        void Progress(float deltaTime);
        void Interrupt(InterruptReason reason);
    }
}
