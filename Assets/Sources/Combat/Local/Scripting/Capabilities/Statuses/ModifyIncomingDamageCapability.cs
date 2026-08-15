using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusModifyIncomingDamageCapability
    {
        DamageModification GetModification(in DamageInstance instance);
    }

    public sealed class ModifyIncomingDamageCapability : IStatusModifyIncomingDamageCapability
    {
        private readonly IIncomingHealDamageModifier _modifier;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly AbilityApiAdapter _skillApiProvider;

        public ModifyIncomingDamageCapability(IIncomingHealDamageModifier modifier, CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider)
        {
            _modifier = modifier;
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
        }

        public DamageModification GetModification(in DamageInstance instance)
        {
            var apiInstance = StatusCapabilityMapper.Adapt(instance, _unitApiAdapter, _skillApiProvider);
            return new(_modifier.GetBonusDamageRecivedValue(apiInstance),
                _modifier.GetBonusDamageRecivedPercent(apiInstance),
                _modifier.GetBonusDamageRecived(apiInstance),
                _modifier.GetDamageFlagMask(apiInstance));
        }
    }
}
