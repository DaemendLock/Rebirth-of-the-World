using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API
{
    // OnScene/External elements
    public interface IActor
    {
        UnitId Id { get; }
        //TODO: Prob tags
    }

    public interface IActionOwner
    {
        float CurrentHealth { get; }
    }

    public interface ITargetable
    {
        float CurrentHealth { get; }

        void ApplyDamage(DTO.ApplyDamageInfo damageInfo);
        void ApplyHealing(DTO.ApplyHealingInfo healingInfo);
    }

    public sealed class UnitNew : IActor, IActionOwner, ITargetable
    {
        private readonly UnitId _id;
        private readonly HealthOwnerFacade _healthOwnerFacade;

        public UnitNew(UnitId id, HealthOwnerFacade healthOwnerFacade)
        {
            _id = id;
            _healthOwnerFacade = healthOwnerFacade;
        }

        public UnitId Id => _id;

        public float CurrentHealth
        {
            get => _healthOwnerFacade.GetHealth(_id).CurrentHealth;
            set => _healthOwnerFacade.SetHealth(_id, value);
        }

        public void ApplyDamage(DTO.ApplyDamageInfo info)
        {
            ApplyDamageInfo applyDamageInfo = new(_id, info.Damage, info.Flags, info.Attacker?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyDamage(applyDamageInfo);
        }

        public void ApplyHealing(DTO.ApplyHealingInfo info)
        {
            ApplyHealingInfo applyHealingInfo = new(_id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }
    }
}
