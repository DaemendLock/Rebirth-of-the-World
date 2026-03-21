using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Local.Domain.Entities.Statuses.Effects
{

    public interface IModifyAttributesStrategy
    {
        AttributesModification ModifyAttributes();
    }

    public readonly ref struct ModifyAttributesEffect
    {
        private readonly IModifyAttributesStrategy _strategy;

        public ModifyAttributesEffect(StatusId id, IModifyAttributesStrategy strategy)
        {
            Id = id;
            _strategy = strategy;
        }

        public StatusId Id { get; }

        public AttributesModification ModifyAttributes() => _strategy.ModifyAttributes();
    }
}
