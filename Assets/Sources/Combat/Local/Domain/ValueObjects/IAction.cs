using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.ValueObjects
{
    public struct ActionData
    {
        public ActionData(float activeTime, float effectiveTime)
        {
            ActiveTime = activeTime;
            EffectiveTime = effectiveTime;
        }

        public float ActiveTime { get; set; }
        public float EffectiveTime { get;set; }
    }

    public interface IAction
    {
        ActionId Id { get; }
        SkillId Source { get; }
        ActionFlags Flags { get; }

        float ActiveTime { get; }
        float EffectiveTime { get; }
        ActionState CurrentState { get; }

        ICollection<EntityId> HittedTargets { get; }

        void Start();
        void Update(ActionData data);

        sealed bool AllowMovement => Flags.HasFlag(ActionFlags.AllowMovement);
        sealed bool CanInterrupt => Flags.HasFlag(ActionFlags.CanInterrupt);
        sealed bool IsActive => CurrentState != ActionState.Inactive;
    }
}
