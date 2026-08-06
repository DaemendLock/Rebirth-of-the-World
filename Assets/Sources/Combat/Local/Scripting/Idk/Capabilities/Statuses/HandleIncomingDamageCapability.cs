using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Idk.Capabilities.Statuses
{
    public readonly ref struct HandleIncomingDamageCapability
    {
        private readonly IIncomingHealDamageHandler _handler;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly AbilityApiAdapter _skillApiProvider;

        public HandleIncomingDamageCapability(IIncomingHealDamageHandler handler, CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider)
        {
            _handler = handler;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public void Handle(in DamageResult result) =>
            _handler.OnTakeDamage(StatusCapabilityMapper.Adapt(result, _unitApiAdapter, _skillApiProvider));
    }
}
