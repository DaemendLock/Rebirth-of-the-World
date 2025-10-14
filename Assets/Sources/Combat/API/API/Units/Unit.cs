using Combat.API.DTO;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;

namespace Combat.API
{
    public sealed class Unit
    {
        private readonly EntityId _id;
        private readonly CharacterController _characterController;
        private readonly HealthOwnerController _healthOwnerController;
        private readonly AttributeOwnerController _attributeOwnerController;

        public Unit(EntityId id,
            CharacterController characterController,
            HealthOwnerController healthOwnerController,
            AttributeOwnerController attributeOwnerController)
        {
            _id = id;

            _characterController = characterController;
            _healthOwnerController = healthOwnerController;
            _attributeOwnerController = attributeOwnerController;
        }

        public EntityId Id => _id;

        public Team Team => _characterController.GetTeam(_id);

        public ModelName ModelName => _characterController.GetPosition(_id).ModelName;

        public float Scale
        {
            get => _characterController.GetPosition(_id).Scale;
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public UnityEngine.Vector3 Position => _characterController.GetPosition(_id).Position;

        public bool Alive => _characterController.IsAlive(_id);

        public float CurrentHealth
        {
            get => _healthOwnerController.GetHealth(_id).CurrentHealth;
            set => _healthOwnerController.SetHealth(_id, value);
        }

        public float MaxHealth => _healthOwnerController.GetHealth(_id).MaxHealth;

        public float GetCooldown(SkillId skillId) => 0;

        public float GetAttributeValue(Attribute attribute) => _attributeOwnerController.GetAttributeValue(_id, attribute);

        public float GetVersalityModifier() => _attributeOwnerController.GetVersalityModifier(_id);

        public float GetHasteModifier() => _attributeOwnerController.GetHasteModifier(_id);

        public bool CanHurt(Unit target) => Team != target.Team;

        public void ApplyStatus(ApplyStatusInfo info) => _characterController.ApplyStatus(_id, info.Name, info.StackCount, info.Duration, info.Source?.SkillId, info.Source?.OwnerId);

        public bool HasStatus(StatusName name) => _characterController.HasStatus(_id, name);

        public float GetResourceValue(ResourceId resource) => _characterController.GetResourceValue(_id, resource);

        public void GiveResource(ResourceId resource, float value, SkillApi source) => _characterController.GiveResource(_id, resource, value, source?.SkillId, source?.OwnerId);

        public void SpendResource(ResourceId resource, float value, SkillApi source) => _characterController.SpendResource(_id, resource, value, source?.SkillId, source?.OwnerId);

        public void ApplyDamage(DTO.ApplyDamageInfo info)
        {
            Local.Controllers.ApplyDamageInfo applyDamageInfo = new(_id, info.Damage, info.Flags, info.Attacker?.Id, info.Source?.SkillId, info.Source?.OwnerId);
            _healthOwnerController.ApplyDamage(applyDamageInfo);
        }

        public void ApplyHealing(DTO.ApplyHealingInfo info)
        {
            Local.Controllers.ApplyHealingInfo applyHealingInfo = new(_id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.SkillId, info.Source?.OwnerId);
            _healthOwnerController.ApplyHealing(applyHealingInfo);
        }

        public void Kill(KillInfo data) => _characterController.Kill(_id, data.Source?.SkillId, data.Source?.OwnerId);

        public void Revive(ReviveInfo data) => _characterController.Revive(_id, data.Source?.SkillId, data.Source?.OwnerId);
    }
}
