using Combat.API;
using Combat.API.Capabilities;
using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.UseCases;
using Combat.Local.Scripting.Capabilities.Units;

using System.Collections.Generic;

namespace Combat.Local.Scripting
{
    public sealed class UnitNew : IActor, IActionOwner, ITargetable
    {
        private readonly UnitId _id;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly CharacterFacade _characterFacade;

        private readonly IStatusCapability _statusCapability;

        private readonly List<ScaleEffectId> _scaleEffectIds;

        public UnitNew(UnitId id, HealthOwnerFacade healthOwnerFacade, CharacterFacade characterFacade, StatusOwnerApplyUseCase statusApplyUseCase, StatusRemoveUseCase statusRemoveUse)
        {
            _id = id;
            _healthOwnerFacade = healthOwnerFacade;
            _characterFacade = characterFacade;

            _statusCapability = new StatusApplyRemoveCapabilty(_id, statusApplyUseCase, statusRemoveUse);
            _scaleEffectIds = new();
        }

        public UnitId Id => _id;

        public float CurrentHealth
        {
            get => _healthOwnerFacade.GetHealth(_id).CurrentHealth;
            set => _healthOwnerFacade.SetHealth(_id, value);
        }

        public void ApplyDamage(API.DTO.ApplyDamageInfo info)
        {
            ApplyDamageInfo applyDamageInfo = new(_id, info.Damage, info.Flags, info.Attacker?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyDamage(applyDamageInfo);
        }

        public void ApplyHealing(API.DTO.ApplyHealingInfo info)
        {
            ApplyHealingInfo applyHealingInfo = new(_id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }

        public ScaleEffectId StartScaleOverTime(float rate) => _characterFacade.StartScaleOverTime(_id, rate);

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
