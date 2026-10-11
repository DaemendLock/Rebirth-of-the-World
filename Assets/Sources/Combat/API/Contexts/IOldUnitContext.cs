using Combat.API.DTO;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using System;

namespace Combat.API.Contexts
{
    public interface IOldUnitContext
    {
        bool Alive { get; }
        float CurrentHealth { get; set; }
        UnitId Id { get; }
        float MaxHealth { get; }
        ModelName ModelName { get; }
        UnityEngine.Vector3 Position { get; }
        float Scale { get; set; }
        Team Team { get; }

        void AddMovement(UnityEngine.Vector3 direction, float speed, bool isRelative);
        void ApplyDamage(ApplyDamageInfo info);
        void ApplyHealing(ApplyHealingInfo info);
        void ApplyStatus(ApplyStatusInfo info);
        bool CanHurt(IOldUnitContext target);
        float GetAttributeValue(UnitAttribute attribute);
        float GetCooldown(SkillId skillId);
        float GetHasteModifier();
        float GetResourceValue(ResourceId resource);
        float GetVersalityModifier();
        void GiveResource(GiveResourceInfo info);
        bool HasStatus(StatusType name);
        void Kill(KillInfo data);
        void Revive(ReviveInfo data);
        void SpendResource(ResourceId resource, float value, AbilityApi source);
    }
}
