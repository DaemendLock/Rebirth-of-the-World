using DaeHitbox;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Services;
using Server.Combat.Domain.Skills;
using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Domain.Entities
{
    public class Unit
    {
        private readonly IHealDamageApplicationService _healDamageApplicationService;
        private readonly IKillReviveService _killReviveService;

        private readonly IHitboxCollection _hitboxes;

        private readonly Killable _killable;
        private readonly Transform _transform;
        private readonly AttributesOwner _attributesOwner;

        public Unit(EntityId id, UnitId unitId, Team team, Killable killable, Transform transform, IHealDamageApplicationService healDamageApplicationService)
        {
            Id = id;
            UnitId = unitId;
            Team = team;
            _killable = killable;
            _transform = transform;
            _healDamageApplicationService = healDamageApplicationService;
        }

        public EntityId Id { get; }

        public UnitId UnitId { get; }

        public Team Team { get; }

        public float Scale
        {
            get => _transform.Scale;
            set => _transform.Scale = value;
        }

        public float CurrentHealth
        {
            get => _killable.CurrentHealth;
            set => _killable.CurrentHealth = value;
        }

        public float MaxHealth => _attributesOwner.GetMaxHealthBonus() + _killable.DefaultHealth;

        public bool Alive
        {
            get => _killable.Alive;
            set => _killable.Alive = value;
        }

        //bool CanMove();

        public void ApplyDamage(DamageData data) => _healDamageApplicationService.ApplyDamage(this, data);

        public void ApplyHealing(HealingData data) => _healDamageApplicationService.ApplyHealing(this, data);

        public void Kill(KillData data) => _killReviveService.Kill(this, data);

        public void Revive(ReviveData data) => _killReviveService.Revive(this, data);

        public IHitbox GetHitbox(HitboxType hitboxType) => _hitboxes.GetHitbox(hitboxType);

        public float GetCooldown(SkillId skillId) => 0;// _skillOwner.GetCooldown(skillId).Left * 0.001f;

        public float GetAttributeValue(Attribute attribute) => _attributesOwner.GetAttributeValue(attribute);
    }
}
