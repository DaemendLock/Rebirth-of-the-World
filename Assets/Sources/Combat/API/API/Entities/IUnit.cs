using Combat.API.DTO;
using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.API
{
    public interface IUnit
    {
        bool Alive { get; }
        float CurrentHealth { get; set; }
        UnitId Id { get; }
        float MaxHealth { get; }
        ModelName ModelName { get; }
        Vector3 Position { get; }
        float Scale { get; set; }
        Team Team { get; }

        void AddMovement(Vector3 direction, float speed, bool isRelative);
        void ApplyDamage(ApplyDamageInfo info);
        void ApplyHealing(ApplyHealingInfo info);
        void ApplyStatus(ApplyStatusInfo info);
        bool CanHurt(IUnit target);
        bool Equals(IUnit other);
        float GetAttributeValue(Attribute attribute);
        float GetCooldown(SkillId skillId);
        float GetHasteModifier();
        float GetResourceValue(ResourceId resource);
        float GetVersalityModifier();
        void GiveResource(GiveResourceInfo info);
        bool HasStatus(StatusType name);
        void Kill(KillInfo data);
        void Revive(ReviveInfo data);
        void SpendResource(ResourceId resource, float value, IAbilityApi source);
    }
}