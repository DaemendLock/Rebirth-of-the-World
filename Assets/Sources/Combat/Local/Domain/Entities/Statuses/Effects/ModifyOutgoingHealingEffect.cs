using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses.Effects
{
    public interface IModifyParentOutgoingHealingStrategy
    {
        HealingModification GetModification(HealingInstance instance);
    }

    public readonly ref struct ModifyOutgoingHealingEffect
    {
        private readonly IModifyParentOutgoingHealingStrategy _strategy;

        public ModifyOutgoingHealingEffect(StatusId status, IModifyParentOutgoingHealingStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public HealingModification GetModification(HealingInstance instance) => _strategy.GetModification(instance);
    }
}
