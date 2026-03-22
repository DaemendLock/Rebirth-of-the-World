using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyParentOutgoingDamageStrategy
    {
        DamageModification GetModification(DamageInstance instance);
    }

    public readonly ref struct ModifyOutgoingDamageEffect
    {
        private readonly IModifyParentOutgoingDamageStrategy _strategy;

        public ModifyOutgoingDamageEffect(StatusId status, IModifyParentOutgoingDamageStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public DamageModification GetModification(DamageInstance instance) => _strategy.GetModification(instance);
    }
}
