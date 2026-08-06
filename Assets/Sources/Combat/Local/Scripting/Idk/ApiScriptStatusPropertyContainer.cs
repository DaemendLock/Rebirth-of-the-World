using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Gateways.Repositories.Statuses
{
    public class ApiScriptStatusPropertyContainer : IStatusPropertyContainer
    {
        private readonly AbilityApiAdapter _abilityApiAdapter;
        private readonly CharacterApiAdapter _characterApiAdapter;
        private readonly StatusScript _statusScript;
        private readonly IIncomingHealDamageHandler _takeDamageEffectStrategy;
        private readonly IOutgoingHealDamageHandler _dealDamageEffectStrategy;
        private readonly IOutgoingDamageModifier _modifyParentOutgoingDamageStrategy;
        private readonly IOutgoingHealingModifier _modifyParentOutgoingHealingStrategy;
        private readonly IIncomingHealDamageModifier _modifyParentIncomingDamageStrategy;
        private readonly IAttributesModifier _modifyAttributesStrategy;
        private readonly ITimeScaleModifier _modifyTimeScaleStrategy;

        public ApiScriptStatusPropertyContainer(StatusScript script, CharacterApiAdapter characterApiAdapter, AbilityApiAdapter abilityApiAdapter)
        {
            _statusScript = script;
            _characterApiAdapter = characterApiAdapter;
            _abilityApiAdapter = abilityApiAdapter;

            if (script is IIncomingHealDamageHandler incomingHealDamageHandler)
            {
                _takeDamageEffectStrategy = incomingHealDamageHandler;
            }

            if (script is IOutgoingHealDamageHandler outgoingHealDamageHandler)
            {
                _dealDamageEffectStrategy = outgoingHealDamageHandler;
            }

            if (script is IOutgoingDamageModifier outgoinDamageModifier)
            {
                _modifyParentOutgoingDamageStrategy = outgoinDamageModifier;
            }

            if (script is IOutgoingHealingModifier outgoinHealingModifier)
            {
                _modifyParentOutgoingHealingStrategy = outgoinHealingModifier;
            }

            if (script is IIncomingHealDamageModifier incomingHealDamageModifier)
            {
                _modifyParentIncomingDamageStrategy = incomingHealDamageModifier;
            }

            if (script is ITimeScaleModifier timeScaleModifier)
            {
                _modifyTimeScaleStrategy = timeScaleModifier;
            }

            if (script is IAttributesModifier attributesModifier)
            {
                _modifyAttributesStrategy = attributesModifier;
            }
        }

        public bool TryGetProperty(out BaseStatusCapabilties effect)
        {
            effect = new(_statusScript);
            return true;
        }

        public bool TryGetProperty(out HandleIncomingDamageCapability effect)
        {
            if (_takeDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_takeDamageEffectStrategy, _characterApiAdapter, _abilityApiAdapter);
            return true;
        }

        public bool TryGetProperty(out HandleOutgoingDamageCapability effect)
        {
            if (_dealDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_dealDamageEffectStrategy, _characterApiAdapter, _abilityApiAdapter);
            return true;
        }

        public bool TryGetProperty(out ModifyOutgoingDamageCapability effect)
        {
            if (_modifyParentOutgoingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_modifyParentOutgoingDamageStrategy, _characterApiAdapter, _abilityApiAdapter);
            return true;
        }

        public bool TryGetProperty(out ModifyOutgoingHealingCapability effect)
        {
            if (_modifyParentOutgoingHealingStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_modifyParentOutgoingHealingStrategy, _characterApiAdapter, _abilityApiAdapter);
            return true;
        }

        public bool TryGetProperty(out ModifyIncomingDamageCapability effect)
        {
            if (_modifyParentIncomingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_modifyParentIncomingDamageStrategy, _characterApiAdapter, _abilityApiAdapter);
            return true;
        }

        public bool TryGetProperty(out ModifyAttributesCapability effect)
        {
            if (_modifyAttributesStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_modifyAttributesStrategy);
            return true;
        }

        public bool TryGetProperty(out ModifyTimeScaleCapability effect)
        {
            if (_modifyTimeScaleStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = new(_modifyTimeScaleStrategy);
            return true;
        }
    }
}
