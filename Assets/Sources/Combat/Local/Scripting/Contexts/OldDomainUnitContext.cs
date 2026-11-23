using Combat.API;
using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.Facades;

using System.Linq;

namespace Combat.Local.Scripting
{
    public sealed class OldDomainUnitContext : IOldUnitContext
    {
        private readonly UnitContext _unit;
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;

        public OldDomainUnitContext(CharacterFacade characterController,
            HealthOwnerFacade healthOwnerController,
            UnitContext unitContext)
        {
            _characterFacade = characterController;
            _healthOwnerFacade = healthOwnerController;
            _unit = unitContext;
        }

        public UnitId Id => _unit.Id;

        public Team Team => _unit.Positionable.Team;

        public ModelName ModelName => _unit.Positionable.ModelName;

        public float Scale
        {
            get => _unit.Positionable.Scale;
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public UnityEngine.Vector3 Position => _unit.Positionable.Position;

        public bool Alive => _unit.LifeState.IsAlive;

        public float CurrentHealth
        {
            get => _unit.Health.Current;
            set => _healthOwnerFacade.SetHealth(Id, value);
        }

        public float MaxHealth => _unit.Health.Max;

        public float CurrentComposure
        {
            get => _unit.Composure.Current;
            set => _healthOwnerFacade.SetComposure(Id, value);
        }

        public float MaxComposure => _unit.Composure.Max;

        public float GetCooldown(SkillId skillId) => 0;

        public float GetAttributeValue(UnitAttribute attribute) => _unit.Attributes.GetAttributeValue(attribute);

        public float GetVersalityModifier() => _unit.Attributes.GetVersalityModifier();

        public float GetHasteModifier() => _unit.Attributes.GetHasteModifier();

        public bool CanHurt(IOldUnitContext target) => Team != target.Team;

        public void ApplyStatus(ApplyStatusInfo info) => _characterFacade.ApplyStatus(Id, info.Name, info.StackCount, info.Duration, info.Source);

        public bool HasStatus(StatusType name) => _unit.Statuses.GetAll().Any(value => value.Name == name);

        public float GetResourceValue(ResourceId resource) => _unit.Resources.GetResource(resource).Value;

        public void GiveResource(GiveResourceInfo info) => _characterFacade.GiveResource(Id, info.Resource, info.Value, info.Source?.AbilityKey);

        public void SpendResource(ResourceId resource, float value, AbilityApi source) => _characterFacade.SpendResource(Id, resource, value, source?.AbilityKey);

        public void ApplyDamage(ApplyDamageInfo info) =>
            _healthOwnerFacade.ApplyDamage(new(Id, info.HealthDamage, info.ComposureDamage, info.Flags, info.Attacker?.Id, info.Source?.AbilityKey));

        public void ApplyHealing(API.DTO.ApplyHealingInfo info)
        {
            Application.Facades.ApplyHealingInfo applyHealingInfo = new(Id, info.Healing, info.Flags, info.Healer?.Id, info.Source?.AbilityKey);
            _healthOwnerFacade.ApplyHealing(applyHealingInfo);
        }

        public void AddMovement(UnityEngine.Vector3 direction, float speed, bool isRelative) => _characterFacade.AddMoveInDirectionEffect(Id, direction, speed, isRelative);

        public void Kill(KillInfo data) => _characterFacade.Kill(Id, data.Source?.AbilityKey);

        public void Revive(ReviveInfo data) => _characterFacade.Revive(Id, data.Source?.AbilityKey);

        public bool Equals(Unit other) => other.Id == Id;
    }
}
