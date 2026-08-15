using Combat.API;
using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.Local.Scripting
{
    public sealed class OldDomainUnitContext : IOldUnitContext
    {
        private readonly UnitId _id;
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        public OldDomainUnitContext(UnitId id,
            CharacterFacade characterController,
            HealthOwnerFacade healthOwnerController,
            AttributeOwnerFacade attributeOwnerController)
        {
            _id = id;

            _characterFacade = characterController;
            _healthOwnerFacade = healthOwnerController;
            _attributeOwnerFacade = attributeOwnerController;
        }

        public UnitId Id => _id;

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

        public float GetAttributeValue(UnitAttribute attribute) => _attributeOwnerFacade.GetAttributeValue(_id, attribute);

        public float GetVersalityModifier() => _attributeOwnerFacade.GetVersalityModifier(_id);

        public float GetHasteModifier() => _attributeOwnerFacade.GetHasteModifier(_id);

        public bool CanHurt(IOldUnitContext target) => Team != target.Team;

        public void ApplyStatus(ApplyStatusInfo info) => _characterFacade.ApplyStatus(_id, info.Name, info.StackCount, info.Duration, info.Source?.AbilityKey);

        public bool HasStatus(StatusType name) => _characterFacade.HasStatus(_id, name);

        public float GetResourceValue(ResourceId resource) => _characterFacade.GetResourceValue(_id, resource);

        public void GiveResource(GiveResourceInfo info) => _characterFacade.GiveResource(_id, info.Resource, info.Value, info.Source?.AbilityKey);

        public void SpendResource(ResourceId resource, float value, AbilityApi source) => _characterFacade.SpendResource(_id, resource, value, source?.AbilityKey);

        public void ApplyDamage(Combat.API.DTO.ApplyDamageInfo info)
        {
            Domain.Facades.ApplyDamageInfo applyDamageInfo = new(_id, info.Damage, info.Flags, info.Attacker?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyDamage(applyDamageInfo);
        }

        public void ApplyHealing(Combat.API.DTO.ApplyHealingInfo info)
        {
            Domain.Facades.ApplyHealingInfo applyHealingInfo = new(_id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }

        public void AddMovement(UnityEngine.Vector3 direction, float speed, bool isRelative) => _characterFacade.AddMoveInDirectionEffect(Id, direction, speed, isRelative);

        public void Kill(KillInfo data) => _characterFacade.Kill(_id, data.Source?.AbilityKey);

        public void Revive(ReviveInfo data) => _characterFacade.Revive(_id, data.Source?.AbilityKey);

        public bool Equals(Unit other) => other.Id == _id;
    }
}
