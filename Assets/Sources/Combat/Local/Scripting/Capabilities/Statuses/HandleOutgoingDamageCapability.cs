using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public readonly ref struct HandleOutgoingDamageCapability
    {
        private readonly IOutgoingHealDamageHandler _handler;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly AbilityApiAdapter _skillApiProvider;

        public HandleOutgoingDamageCapability(IOutgoingHealDamageHandler handler, CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider)
        {
            _handler = handler;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public void Handle(in DamageResult result) =>
            _handler.OnDealDamage(StatusCapabilityMapper.Adapt(result, _unitApiAdapter, _skillApiProvider));
    }
}
