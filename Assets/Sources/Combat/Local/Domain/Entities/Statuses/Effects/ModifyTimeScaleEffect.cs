using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyTimeScaleStrategy
    {
        float GetModification();
    }

    public readonly ref struct ModifyTimeScaleEffect
    {
        private readonly IModifyTimeScaleStrategy _strategy;

        public ModifyTimeScaleEffect(StatusId status, IModifyTimeScaleStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public float GetModification() => _strategy.GetModification();
    }
}
