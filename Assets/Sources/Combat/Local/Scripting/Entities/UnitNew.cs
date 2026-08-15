using Combat.API;
using Combat.Common.Primitives;
using Combat.Local.Domain.Facades;

namespace Combat.Local.Scripting
{
    public sealed class UnitNew : IActor, IActionOwner, ITargetable
    {
        private readonly UnitId _id;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly CharacterFacade _characterFacade;

        public UnitNew(UnitId id, HealthOwnerFacade healthOwnerFacade, CharacterFacade characterFacade)
        {
            _id = id;
            _healthOwnerFacade = healthOwnerFacade;
            _characterFacade = characterFacade;
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
    }
}
