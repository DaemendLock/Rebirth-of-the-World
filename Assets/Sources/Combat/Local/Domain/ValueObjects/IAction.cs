using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.ValueObjects
{

    public readonly struct ActionData
    {
        public ActionData(bool isActive, float activeTime, float effectiveTime)
        {
            IsActive = isActive;
            ActiveTime = activeTime;
            EffectiveTime = effectiveTime;
        }

        public bool IsActive { get; }
        public float ActiveTime { get; }
        public float EffectiveTime { get; }
    }

    public interface IAction
    {
        ActionId Id { get; }
        ActionFlags Flags { get; }

        float ActiveTime { get; }
        float EffectiveTime { get; }
        ActionState CurrentState { get; }

        sealed bool AllowMovement => Flags.HasFlag(ActionFlags.AllowMovement);
        sealed bool CanInterrupt => Flags.HasFlag(ActionFlags.CanInterrupt);

        ICollection<EntityId> HittedTargets { get; }

        void Start();
        void Update(ActionData data);

        bool IsActive => CurrentState != ActionState.Inactive;
    }
}
