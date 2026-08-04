using Combat.API;
using Combat.API.Adapters;
using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Gateways.Repositories.Statuses
{
    public class ApiScriptStatusPropertyContainer : IStatusPropertyContainer
    {
        private readonly StatusScript _statusScript;
        private readonly IIncomingHealDamageHandler _takeDamageEffectStrategy;
        private readonly IOutgoingHealDamageHandler _dealDamageEffectStrategy;
        private readonly IOutgoingDamageModifier _modifyParentOutgoingDamageStrategy;
        private readonly IOutgoingHealingModifier _modifyParentOutgoingHealingStrategy;
        private readonly IIncomingHealDamageModifier _modifyParentIncomingDamageStrategy;
        private readonly IAttributesModifier _modifyAttributesStrategy;
        private readonly ITimeScaleModifier _modifyTimeScaleStrategy;

        public ApiScriptStatusPropertyContainer(StatusScript script)
        {
            _statusScript = script;

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

        public bool DestroyOnExpire => true;

        public void Apply() => _statusScript.OnCreate();

        public void Expire() => _statusScript.OnExpire();

        public void Remove() => _statusScript.OnRemove();

        public void Tick() => _statusScript.OnTick();

        public bool TryGetProperty(out IIncomingHealDamageHandler effect)
        {
            if (_takeDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _takeDamageEffectStrategy;
            return true;
        }

        public bool TryGetProperty(out IOutgoingHealDamageHandler effect)
        {
            if (_dealDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _dealDamageEffectStrategy;
            return true;
        }

        public bool TryGetProperty(out IOutgoingDamageModifier effect)
        {
            if (_modifyParentOutgoingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentOutgoingDamageStrategy;
            return true;
        }

        public bool TryGetProperty(out IOutgoingHealingModifier effect)
        {
            if (_modifyParentOutgoingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentOutgoingHealingStrategy;
            return true;
        }

        public bool TryGetProperty(out IIncomingHealDamageModifier effect)
        {
            if (_modifyParentIncomingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentIncomingDamageStrategy;
            return true;
        }

        public bool TryGetProperty(out IAttributesModifier effect)
        {
            if (_modifyAttributesStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyAttributesStrategy;
            return true;
        }

        public bool TryGetProperty(out ITimeScaleModifier effect)
        {
            if (_modifyTimeScaleStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyTimeScaleStrategy;
            return true;
        }
    }
}
