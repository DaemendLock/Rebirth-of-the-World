using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public class OldStatusCapabilityProvider : IStatusCapabilityProvider
    {
        private readonly IStatusLifecycleCapability _lifecycleCapability;
        private readonly IStatusTickCapability _tickCapability;
        private readonly IStatusHandleIncomingDamageCapability _handleIncomingDamageCapability;
        private readonly IStatusHandleOutgoingDamageCapability _handleOutgoingDamageCapability;
        private readonly IStatusModifyOutgoingDamageCapability _modifyOutgoingDamageCapability;
        private readonly IStatusModifyOutgoingHealingCapability _modifyOutgoingHealingCapability;
        private readonly IStatusModifyIncomingDamageCapability _modifyIncomingDamageCapability;
        private readonly IStatusModifyAttributesCapability _modifyAttributesCapability;
        private readonly IStatusModifyTimeScaleCapability _modifyTimeScaleCapability;

        public OldStatusCapabilityProvider(StatusScript script, CharacterApiAdapter characterApiAdapter, AbilityApiAdapter abilityApiAdapter)
        {
            _lifecycleCapability = new OldStatusLifecycleCapability(script);
            _tickCapability = new OldStatusTickCapability(script);

            if (script is IIncomingHealDamageHandler incomingHealDamageHandler)
            {
                _handleIncomingDamageCapability = new HandleIncomingDamageCapability(incomingHealDamageHandler, characterApiAdapter, abilityApiAdapter);
            }

            if (script is IOutgoingHealDamageHandler outgoingHealDamageHandler)
            {
                _handleOutgoingDamageCapability = new HandleOutgoingDamageCapability(outgoingHealDamageHandler, characterApiAdapter, abilityApiAdapter);
            }

            if (script is IOutgoingDamageModifier outgoinDamageModifier)
            {
                _modifyOutgoingDamageCapability = new ModifyOutgoingDamageCapability(outgoinDamageModifier, characterApiAdapter, abilityApiAdapter);
            }

            if (script is IOutgoingHealingModifier outgoinHealingModifier)
            {
                _modifyOutgoingHealingCapability = new ModifyOutgoingHealingCapability(outgoinHealingModifier, characterApiAdapter, abilityApiAdapter);
            }

            if (script is IIncomingHealDamageModifier incomingHealDamageModifier)
            {
                _modifyIncomingDamageCapability = new ModifyIncomingDamageCapability(incomingHealDamageModifier, characterApiAdapter, abilityApiAdapter);
            }

            if (script is ITimeScaleModifier timeScaleModifier)
            {
                _modifyTimeScaleCapability = new ModifyTimeScaleCapability(timeScaleModifier);
            }

            if (script is IAttributesModifier attributesModifier)
            {
                _modifyAttributesCapability = new ModifyAttributesCapability(attributesModifier);
            }
        }

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(IStatusLifecycleCapability))
            {
                return _lifecycleCapability as T;
            }

            if (typeof(T) == typeof(IStatusTickCapability))
            {
                return _tickCapability as T;
            }

            if (typeof(T) == typeof(IStatusHandleIncomingDamageCapability))
            {
                return _handleIncomingDamageCapability as T;
            }

            if (typeof(T) == typeof(IStatusHandleOutgoingDamageCapability))
            {
                return _handleOutgoingDamageCapability as T;
            }

            if (typeof(T) == typeof(IStatusModifyOutgoingDamageCapability))
            {
                return _modifyOutgoingDamageCapability as T;
            }

            if (typeof(T) == typeof(IStatusModifyOutgoingHealingCapability))
            {
                return _modifyOutgoingHealingCapability as T;
            }

            if (typeof(T) == typeof(IStatusModifyIncomingDamageCapability))
            {
                return _modifyIncomingDamageCapability as T;
            }

            if (typeof(T) == typeof(IStatusModifyAttributesCapability))
            {
                return _modifyAttributesCapability as T;
            }

            if (typeof(T) == typeof(IStatusModifyTimeScaleCapability))
            {
                return _modifyTimeScaleCapability as T;
            }

            return null;
        }
    }
}
