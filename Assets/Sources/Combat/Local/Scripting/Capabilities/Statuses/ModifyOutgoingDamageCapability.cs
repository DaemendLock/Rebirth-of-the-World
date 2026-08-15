using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusModifyOutgoingDamageCapability
    {
        DamageModification GetModification(in DamageInstance instance);
    }

    public sealed class ModifyOutgoingDamageCapability : IStatusModifyOutgoingDamageCapability
    {
        private readonly IOutgoingDamageModifier _modifier;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly AbilityApiAdapter _skillApiProvider;

        public ModifyOutgoingDamageCapability(IOutgoingDamageModifier modifier, CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider)
        {
            _modifier = modifier;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public DamageModification GetModification(in DamageInstance instance)
        {
            var apiInstance = StatusCapabilityMapper.Adapt(instance, _unitApiAdapter, _skillApiProvider);
            return new(_modifier.GetDamageDealthModification_Value(apiInstance),
                _modifier.GetDamageDealthModification_Percent(apiInstance),
                _modifier.GetDamageDealthModification_Bonus(apiInstance),
                _modifier.GetDamageFlagMask(apiInstance));
        }
    }
}
