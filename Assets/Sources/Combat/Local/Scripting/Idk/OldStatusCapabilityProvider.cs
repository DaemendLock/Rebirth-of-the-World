using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

using System;

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

        public bool TryGetCapability<T>(out T capability) where T : class
        {
            if (typeof(T) == typeof(IStatusLifecycleCapability))
            {
                capability = _lifecycleCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusTickCapability))
            {
                capability = _tickCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusHandleIncomingDamageCapability))
            {
                capability = _handleIncomingDamageCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusHandleOutgoingDamageCapability))
            {
                capability = _handleOutgoingDamageCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusModifyOutgoingDamageCapability))
            {
                capability = _modifyOutgoingDamageCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusModifyOutgoingHealingCapability))
            {
                capability = _modifyOutgoingHealingCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusModifyIncomingDamageCapability))
            {
                capability = _modifyIncomingDamageCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusModifyAttributesCapability))
            {
                capability = _modifyAttributesCapability as T;
                return capability != null;
            }

            if (typeof(T) == typeof(IStatusModifyTimeScaleCapability))
            {
                capability = _modifyTimeScaleCapability as T;
                return capability != null;
            }

            capability = null;
            return false;
        }
    }
}
