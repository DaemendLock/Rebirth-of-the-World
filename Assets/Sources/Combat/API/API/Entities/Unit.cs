using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.API
{
    public sealed class Unit
    {
        private readonly IOldUnitContext _unitContext;

        public Unit(IOldUnitContext unitContext)
        {
            _unitContext = unitContext;
        }

        public bool Alive => _unitContext.Alive;

        public float CurrentHealth
        {
            get => _unitContext.CurrentHealth;
            set => _unitContext.CurrentHealth = value;
        }

        public UnitId Id => _unitContext.Id;

        public float MaxHealth => _unitContext.MaxHealth;

        public ModelName ModelName => _unitContext.ModelName;

        public UnityEngine.Vector3 Position => _unitContext.Position;

        public float Scale => _unitContext.Scale;

        public Team Team => _unitContext.Team;

        public void AddMovement(UnityEngine.Vector3 direction, float speed, bool isRelative) => _unitContext.AddMovement(direction, speed, isRelative);

        public void ApplyDamage(ApplyDamageInfo info) => _unitContext.ApplyDamage(info);

        public void ApplyHealing(ApplyHealingInfo info) => _unitContext.ApplyHealing(info);

        public void ApplyStatus(ApplyStatusInfo info) => _unitContext.ApplyStatus(info);

        public bool CanHurt(Unit target) => _unitContext.CanHurt(target._unitContext);

        public float GetAttributeValue(Common.ValueObjects.UnitAttribute attribute) => _unitContext.GetAttributeValue(attribute);

        public float GetCooldown(SkillId skillId) => _unitContext.GetCooldown(skillId);

        public float GetHasteModifier() => _unitContext.GetHasteModifier();

        public float GetResourceValue(ResourceId resource) => _unitContext.GetResourceValue(resource);

        public float GetVersalityModifier() => _unitContext.GetVersalityModifier();

        public void GiveResource(GiveResourceInfo info) => _unitContext.GiveResource(info);

        public bool HasStatus(StatusType name) => _unitContext.HasStatus(name);

        public void Kill(KillInfo data) => _unitContext.Kill(data);

        public void Revive(ReviveInfo data) => _unitContext.Revive(data);

        public void SpendResource(ResourceId resource, float value, AbilityApi source) => _unitContext.SpendResource(resource, value, source);

        public bool Equals(Unit other) => _unitContext.Id == other._unitContext.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Unit left, Unit right) => left.Equals(right);

        public static bool operator !=(Unit left, Unit right) => !(left == right);
    }
}
