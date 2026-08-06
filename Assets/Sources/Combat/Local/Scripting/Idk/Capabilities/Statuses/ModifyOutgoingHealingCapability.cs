using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Idk.Capabilities.Statuses
{
    public readonly ref struct ModifyOutgoingHealingCapability
    {
        private readonly IOutgoingHealingModifier _modifier;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly AbilityApiAdapter _skillApiProvider;

        public ModifyOutgoingHealingCapability(IOutgoingHealingModifier modifier, CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider)
        {
            _modifier = modifier;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public HealingModification GetModification(in HealingInstance instance)
        {
            var apiInstance = StatusCapabilityMapper.Adapt(instance, _unitApiAdapter, _skillApiProvider);
            return new(_modifier.GetBonusHealingDealthValue(apiInstance),
                _modifier.GetBonusHealingDealthPercent(apiInstance),
                _modifier.GetBonusHealingDealth(apiInstance),
                _modifier.GetHealingFlagMask(apiInstance));
        }
    }
}
