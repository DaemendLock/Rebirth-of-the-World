using Combat.API;
using Combat.API.Adapters;
using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Gateways.Repositories.Statuses
{
    public class ApiScriptStatusPropertyContainer : IStatusPropertyContainer
    {
        private readonly StatusId _statusId;
        private readonly IStatusRepository _statusRepository;
        private readonly StatusScript _statusScript;
        private readonly IStatusApiAdapter _statusApiAdapter;
        private readonly ITakeDamageEffectStrategy _takeDamageEffectStrategy;
        private readonly IDealDamageEffectStrategy _dealDamageEffectStrategy;
        private readonly IModifyParentOutgoingDamageStrategy _modifyParentOutgoingDamageStrategy;
        private readonly IModifyParentOutgoingHealingStrategy _modifyParentOutgoingHealingStrategy;
        private readonly IModifyParentIncomingDamageStrategy _modifyParentIncomingDamageStrategy;
        private readonly IModifyAttributesStrategy _modifyAttributesStrategy;
        private readonly IModifyTimeScaleStrategy _modifyTimeScaleStrategy;

        public ApiScriptStatusPropertyContainer(StatusId statusId, StatusScript script, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider, ISceneApiAdapter sceneApiProvider, IStatusApiAdapter statusApiFactory, IStatusRepository statusRepository)
        {
            _statusId = statusId;
            _statusScript = script;

            if (script is IIncomingHealDamageHandler incomingHealDamageHandler)
            {
                _takeDamageEffectStrategy = new DataDrivenTakeDamageEffectStrategy(incomingHealDamageHandler, unitApiAdapter, skillApiProvider);
            }

            if (script is IOutgoingHealDamageHandler outgoingHealDamageHandler)
            {
                _dealDamageEffectStrategy = new DataDrivenDealDamageEffectStrategy(outgoingHealDamageHandler, unitApiAdapter, skillApiProvider);
            }

            if (script is IOutgoingDamageModifier outgoinDamageModifier)
            {
                _modifyParentOutgoingDamageStrategy = new DataDrivenModifyParentDamageEffectStrategy(outgoinDamageModifier, unitApiAdapter, skillApiProvider);
            }

            if (script is IOutgoingHealingModifier outgoinHealingModifier)
            {
                _modifyParentOutgoingHealingStrategy = new DataDrivenModifyParentHealingEffectStrategy(outgoinHealingModifier, unitApiAdapter, skillApiProvider);
            }

            if (script is IIncomingHealDamageModifier incomingHealDamageModifier)
            {
                _modifyParentIncomingDamageStrategy = new DataDrivenModifyParentIncomingDamageEffectStrategy(incomingHealDamageModifier, unitApiAdapter, skillApiProvider);
            }

            if (script is ITimeScaleModifier timeScaleModifier)
            {
                _modifyTimeScaleStrategy = new DataDrivenModifyTimeScaleEffectStrategy(timeScaleModifier);
            }

            if (script is IAttributesModifier attributesModifier)
            {
                _modifyAttributesStrategy = new DataDrivenModifyAttributeStrategy(attributesModifier);
            }

            _statusApiAdapter = statusApiFactory;
            _statusRepository = statusRepository;
        }

        public bool DestroyOnExpire => true;

        public void Apply()
        {
            _statusScript.OnCreate();
        }

        public void Expire() => _statusScript.OnExpire();

        public void Remove() => _statusScript.OnRemove();

        public void Tick() => _statusScript.OnTick();

        public bool TryGetProperty(out ITakeDamageEffectStrategy effect)
        {
            if (_takeDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _takeDamageEffectStrategy;
            return true;
        }

        public bool TryGetProperty(out IDealDamageEffectStrategy effect)
        {
            if (_dealDamageEffectStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _dealDamageEffectStrategy;
            return true;
        }

        public bool TryGetProperty(out IModifyParentOutgoingDamageStrategy effect)
        {
            if (_modifyParentOutgoingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentOutgoingDamageStrategy;
            return true;
        }

        public bool TryGetProperty(out IModifyParentOutgoingHealingStrategy effect)
        {
            if (_modifyParentOutgoingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentOutgoingHealingStrategy;
            return true;
        }

        public bool TryGetProperty(out IModifyParentIncomingDamageStrategy effect)
        {
            if (_modifyParentIncomingDamageStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyParentIncomingDamageStrategy;
            return true;
        }

        public bool TryGetProperty(out IModifyAttributesStrategy effect)
        {
            if (_modifyAttributesStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyAttributesStrategy;
            return true;
        }

        public bool TryGetProperty(out IModifyTimeScaleStrategy effect)
        {
            if (_modifyTimeScaleStrategy == null)
            {
                effect = default;
                return false;
            }

            effect = _modifyTimeScaleStrategy;
            return true;
        }

        private class DataDrivenTakeDamageEffectStrategy : ITakeDamageEffectStrategy
        {
            private readonly ICharacterApiAdapter _unitApiAdapter;
            private readonly IAbilityApiAdapter _skillApiProvider;
            private readonly IIncomingHealDamageHandler _handler;

            public DataDrivenTakeDamageEffectStrategy(IIncomingHealDamageHandler handler, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
            {
                _handler = handler;
                _unitApiAdapter = unitApiAdapter;
                _skillApiProvider = skillApiProvider;
            }

            public void HandleDamage(DamageResult @event)
            {
                Unit target = _unitApiAdapter.Adaptee(@event.Target);
                Unit attacker = @event.Attacker.HasValue ? _unitApiAdapter.Adaptee(@event.Attacker.Value) : null;
                AbilityApi source;

                if (@event.Skill.HasValue)
                {
                    source = _skillApiProvider.Adaptee(@event.Skill.Value);
                }
                else
                {
                    source = null;
                }

                DamageRecord record = new(target, @event.OriginalDamage, @event.FinalDamage, @event.Flags, attacker, source);
                _handler.OnTakeDamage(record);
            }
        }

        private class DataDrivenDealDamageEffectStrategy : IDealDamageEffectStrategy
        {
            private readonly ICharacterApiAdapter _unitApiAdapter;
            private readonly IAbilityApiAdapter _skillApiProvider;
            private readonly IOutgoingHealDamageHandler _handler;

            public DataDrivenDealDamageEffectStrategy(IOutgoingHealDamageHandler handler, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
            {
                _handler = handler;
                _unitApiAdapter = unitApiAdapter;
                _skillApiProvider = skillApiProvider;
            }

            public void HandleDamage(DamageResult @event)
            {
                Unit target = _unitApiAdapter.Adaptee(@event.Target);
                Unit attacker = @event.Attacker.HasValue ? _unitApiAdapter.Adaptee(@event.Attacker.Value) : null;
                AbilityApi source;

                if (@event.Skill.HasValue)
                {
                    source = _skillApiProvider.Adaptee(@event.Skill.Value);
                }
                else
                {
                    source = null;
                }

                DamageRecord record = new(target, @event.OriginalDamage, @event.FinalDamage, @event.Flags, attacker, source);
                _handler.OnDealDamage(record);
            }
        }

        private class DataDrivenModifyParentDamageEffectStrategy : IModifyParentOutgoingDamageStrategy
        {
            private readonly ICharacterApiAdapter _unitApiAdapter;
            private readonly IAbilityApiAdapter _skillApiProvider;
            private readonly IOutgoingDamageModifier _modifer;

            public DataDrivenModifyParentDamageEffectStrategy(IOutgoingDamageModifier modifier, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
            {
                _modifer = modifier;
                _unitApiAdapter = unitApiAdapter;
                _skillApiProvider = skillApiProvider;
            }

            public DamageModification GetModification(DamageInstance instance)
            {
                Unit target = _unitApiAdapter.Adaptee(instance.Target);
                Unit attacker = instance.Attacker.HasValue ? _unitApiAdapter.Adaptee(instance.Attacker.Value) : null;
                AbilityApi source;

                if (instance.Source.HasValue)
                {
                    source = _skillApiProvider.Adaptee(instance.Source.Value);
                }
                else
                {
                    source = null;
                }

                DamageInstanceApi instanceApi = new(target, attacker, source, instance.OriginalDamage, instance.Flags);
                DamageModification modification = new(_modifer.GetDamageDealthModification_Value(instanceApi), _modifer.GetDamageDealthModification_Percent(instanceApi), _modifer.GetDamageDealthModification_Bonus(instanceApi), _modifer.GetDamageFlagMask(instanceApi));

                return modification;
            }
        }

        private class DataDrivenModifyParentHealingEffectStrategy : IModifyParentOutgoingHealingStrategy
        {
            private readonly ICharacterApiAdapter _unitApiAdapter;
            private readonly IAbilityApiAdapter _skillApiProvider;
            private readonly IOutgoingHealingModifier _modifer;

            public DataDrivenModifyParentHealingEffectStrategy(IOutgoingHealingModifier modifier, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
            {
                _modifer = modifier;
                _unitApiAdapter = unitApiAdapter;
                _skillApiProvider = skillApiProvider;
            }

            public HealingModification GetModification(HealingInstance instance)
            {
                Unit target = _unitApiAdapter.Adaptee(instance.Target);
                Unit attacker = instance.Healer.HasValue ? _unitApiAdapter.Adaptee(instance.Healer.Value) : null;
                AbilityApi source;

                if (instance.Source.HasValue)
                {
                    source = _skillApiProvider.Adaptee(instance.Source.Value);
                }
                else
                {
                    source = null;
                }

                HealingInstanceApi instanceApi = new(target, attacker, source, instance.OriginalHealing, instance.Flags);
                HealingModification modification = new(_modifer.GetBonusHealingDealthValue(instanceApi), _modifer.GetBonusHealingDealthPercent(instanceApi), _modifer.GetBonusHealingDealth(instanceApi), _modifer.GetHealingFlagMask(instanceApi));

                return modification;
            }
        }

        private class DataDrivenModifyParentIncomingDamageEffectStrategy : IModifyParentIncomingDamageStrategy
        {
            private readonly ICharacterApiAdapter _unitApiAdapter;
            private readonly IAbilityApiAdapter _skillApiProvider;
            private readonly IIncomingHealDamageModifier _modifer;

            public DataDrivenModifyParentIncomingDamageEffectStrategy(IIncomingHealDamageModifier modifier, ICharacterApiAdapter unitApiAdapter, IAbilityApiAdapter skillApiProvider)
            {
                _modifer = modifier;
                _unitApiAdapter = unitApiAdapter;
                _skillApiProvider = skillApiProvider;
            }

            public DamageModification GetModification(DamageInstance instance)
            {
                Unit target = _unitApiAdapter.Adaptee(instance.Target);
                Unit attacker = instance.Attacker.HasValue ? _unitApiAdapter.Adaptee(instance.Attacker.Value) : null;
                AbilityApi source;

                if (instance.Source.HasValue)
                {
                    source = _skillApiProvider.Adaptee(instance.Source.Value);
                }
                else
                {
                    source = null;
                }

                DamageInstanceApi instanceApi = new(target, attacker, source, instance.OriginalDamage, instance.Flags);
                DamageModification modification = new(_modifer.GetBonusDamageRecivedValue(instanceApi), _modifer.GetBonusDamageRecivedPercent(instanceApi), _modifer.GetBonusDamageRecived(instanceApi), _modifer.GetDamageFlagMask(instanceApi));

                return modification;
            }
        }

        private class DataDrivenModifyTimeScaleEffectStrategy : IModifyTimeScaleStrategy
        {
            private readonly ITimeScaleModifier _modifer;

            public DataDrivenModifyTimeScaleEffectStrategy(ITimeScaleModifier modifier)
            {
                _modifer = modifier;
            }

            public float GetModification() => _modifer.GetTimeModification();
        }

        private class DataDrivenModifyAttributeStrategy : IModifyAttributesStrategy
        {
            private readonly IAttributesModifier _modifer;

            public DataDrivenModifyAttributeStrategy(IAttributesModifier modifier)
            {
                _modifer = modifier;
            }

            public AttributesModification ModifyAttributes()
            {
                AttributesModification result = new();

                System.Span<AttributeValue> baseValues = stackalloc AttributeValue[AttributesOwner.AttributeCount];
                System.Span<AttributeValue> bonusValues = stackalloc AttributeValue[baseValues.Length];

                baseValues.Clear();
                bonusValues.Clear();

                AttributesData attributesData = new(baseValues, bonusValues);
                _modifer.GetAttributesBonuses(attributesData);

                result.Attack = new(bonusValues[(int)Attribute.Atk]);
                result.Spellpower = new(bonusValues[(int)Attribute.Spellpower]);
                result.Speed = new(bonusValues[(int)Attribute.Speed]);

                return result;
            }
        }
    }
}
