using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyParentIncomingDamageStrategy
    {
        DamageModification GetModification(DamageInstance instance);
    }

    public readonly ref struct ModifyIncomingDamageEffect
    {
        private readonly IModifyParentIncomingDamageStrategy _strategy;

        public ModifyIncomingDamageEffect(StatusId status, IModifyParentIncomingDamageStrategy strategy)
        {
            Status = status;
            _strategy = strategy;
        }

        public StatusId Status { get; }

        public DamageModification GetModification(DamageInstance instance) => _strategy.GetModification(instance);
    }
}
