using Combat.API;
using Combat.API.Capabilities;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.Facades;
using Combat.Local.Application.UseCases;
using Combat.Local.Scripting.Capabilities.Units;

using System.Collections.Generic;

namespace Combat.Local.Scripting
{
    public sealed class UnitNew : IActor, IActionOwner, ITargetable
    {
        private readonly UnitContext _unit;

        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly CharacterFacade _characterFacade;

        private readonly IStatusCapability _statusCapability;

        private readonly List<ScaleEffectId> _scaleEffectIds;

        public UnitNew(UnitContext unit, HealthOwnerFacade healthOwnerFacade, CharacterFacade characterFacade, StatusOwnerApplyUseCase statusApplyUseCase, StatusOwnerRemoveUseCase statusRemoveUse)
        {
            _unit = unit;
            _healthOwnerFacade = healthOwnerFacade;
            _characterFacade = characterFacade;

            _statusCapability = new StatusApplyRemoveCapabilty(Id, statusApplyUseCase, statusRemoveUse);
            _scaleEffectIds = new();
        }

        public UnitId Id => _unit.Id;

        public float CurrentHealth
        {
            get => _unit.Health.Current;
            set => _healthOwnerFacade.SetHealth(Id, value);
        }

        public void ApplyDamage(API.DTO.ApplyDamageInfo info)
        {
            Application.DTO.ApplyDamageInfo applyDamageInfo = new(Id, info.HealthDamage, info.ComposureDamage, info.Flags, info.Attacker?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyDamage(applyDamageInfo);
        }

        public float CurrentComposure
        {
            get => _unit.Composure.Current;
            set => _healthOwnerFacade.SetComposure(Id, value);
        }

        public float MaxComposure => _unit.Health.Max;

        public void ApplyHealing(API.DTO.ApplyHealingInfo info)
        {
            ApplyHealingInfo applyHealingInfo = new(Id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }

        public ScaleEffectId StartScaleOverTime(float rate) => _characterFacade.StartScaleOverTime(Id, rate);

        public void StopScaleOverTime(ScaleEffectId id) => _characterFacade.StopScaleOverTime(id);

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(IStatusCapability))
            {
                return _statusCapability as T;
            }

            return null;
        }

        public void Cleanup()
        {
            foreach (var id in _scaleEffectIds)
            {
                _characterFacade.StopScaleOverTime(id);
            }

            _scaleEffectIds.Clear();
        }
    }
}
