using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public readonly struct ActionData
    {
        public ActionData(SkillId skill, bool isActive, float activeTime, float effectiveTime, bool allowMovement)
        {
            Skill = skill;
            IsActive = isActive;
            ActiveTime = activeTime;
            EffectiveTime = effectiveTime;
            AllowMovement = allowMovement;
        }

        public SkillId Skill { get; }
        public bool IsActive { get; }
        public float ActiveTime { get; }
        public float EffectiveTime { get; }
        public bool AllowMovement { get; }
    }

    public interface IAction
    {
        ICollection<EntityId> HittedTargets { get; }

        SkillId Skill { get; }
        bool IsActive { get; }
        float ActiveTime { get; }
        ActionState CurrentState { get; }
        bool AllowMovement { get; }
        void Start();
        void Update(ActionData data);
    }
}
