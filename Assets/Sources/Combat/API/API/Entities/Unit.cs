using Combat.API.DTO;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API
{
    public sealed class Unit
    {
        private readonly EntityId _id;
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        public Unit(EntityId id,
            CharacterFacade characterController,
            HealthOwnerFacade healthOwnerController,
            AttributeOwnerFacade attributeOwnerController)
        {
            _id = id;

            _characterFacade = characterController;
            _healthOwnerFacade = healthOwnerController;
            _attributeOwnerFacade = attributeOwnerController;
        }

        public EntityId Id => _id;

        public Team Team => _characterFacade.GetTeam(_id);

        public ModelName ModelName => _characterFacade.GetPosition(_id).ModelName;

        public float Scale
        {
            get => _characterFacade.GetPosition(_id).Scale;
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public UnityEngine.Vector3 Position => _characterFacade.GetPosition(_id).Position;

        public bool Alive => _characterFacade.IsAlive(_id);

        public float CurrentHealth
        {
            get => _healthOwnerFacade.GetHealth(_id).CurrentHealth;
            set => _healthOwnerFacade.SetHealth(_id, value);
        }

        public float MaxHealth => _healthOwnerFacade.GetHealth(_id).MaxHealth;

        public float GetCooldown(SkillId skillId) => 0;

        public float GetAttributeValue(Attribute attribute) => _attributeOwnerFacade.GetAttributeValue(_id, attribute);

        public float GetVersalityModifier() => _attributeOwnerFacade.GetVersalityModifier(_id);

        public float GetHasteModifier() => _attributeOwnerFacade.GetHasteModifier(_id);

        public bool CanHurt(Unit target) => Team != target.Team;

        public void ApplyStatus(ApplyStatusInfo info) => _characterFacade.ApplyStatus(_id, info.Name, info.StackCount, info.Duration, info.Source?.SkillId, info.Source?.OwnerId);

        public bool HasStatus(StatusName name) => _characterFacade.HasStatus(_id, name);

        public float GetResourceValue(ResourceId resource) => _characterFacade.GetResourceValue(_id, resource);

        public void GiveResource(GiveResourceInfo info) => _characterFacade.GiveResource(_id, info.Resource, info.Value, info.Source?.SkillId, info.Source?.OwnerId);

        public void SpendResource(ResourceId resource, float value, SkillApi source) => _characterFacade.SpendResource(_id, resource, value, source?.SkillId, source?.OwnerId);

        public void ApplyDamage(DTO.ApplyDamageInfo info)
        {
            Local.Domain.Facades.ApplyDamageInfo applyDamageInfo = new(_id, info.Damage, info.Flags, info.Attacker?.Id, info.Source?.SkillId, info.Source?.OwnerId);
            _healthOwnerFacade.ApplyDamage(applyDamageInfo);
        }

        public void ApplyHealing(DTO.ApplyHealingInfo info)
        {
            Local.Domain.Facades.ApplyHealingInfo applyHealingInfo = new(_id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.SkillId, info.Source?.OwnerId);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }

        public void Kill(KillInfo data) => _characterFacade.Kill(_id, data.Source?.SkillId, data.Source?.OwnerId);

        public void Revive(ReviveInfo data) => _characterFacade.Revive(_id, data.Source?.SkillId, data.Source?.OwnerId);

        public bool Equals(Unit other) => other.Id == _id;
    }
}
