using Combat.API.Contexts;
using Combat.API.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public interface IStatusHandleIncomingDamageCapability
    {
        void Handle(IStatusContext context, in DamageResult result);
    }

    public sealed class HandleIncomingDamageCapability : IStatusHandleIncomingDamageCapability
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

        public void Handle(IStatusContext context, in DamageResult result) =>
            _handler.OnTakeDamage(StatusCapabilityMapper.Adapt(result, _unitApiAdapter, _skillApiProvider));
    }
}
