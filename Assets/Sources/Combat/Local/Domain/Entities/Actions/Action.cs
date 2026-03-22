using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public class Action
    {
        private readonly IActionStrategy _strategy;
        private readonly List<EntityId> _hittedTargets;

        public Action(ActionId id, SkillId source, ActionFlags flags, IActionStrategy strategy)
        {
            Id = id;
            Source = source;
            Flags = flags;
            _strategy = strategy;

            _hittedTargets = new();
            ActiveTime = 0;
        }

        public ActionId Id { get; }

        public SkillId Source { get; }

        public ActionFlags Flags { get; }

        public float ActiveTime { get; set; }

        public float EffectiveTime => _strategy.EffectiveTime;

        public ActionState CurrentState => _strategy.State;

        public ICollection<EntityId> HittedTargets => _hittedTargets;

        public void Start() => _strategy.Start();

        public void Update(float deltaTime)
        {
            ActiveTime += deltaTime;
            _strategy.Progress(deltaTime);
        }

        public bool AllowMovement => Flags.HasFlag(ActionFlags.AllowMovement);
        public bool CanInterrupt => Flags.HasFlag(ActionFlags.CanInterrupt);
        public bool IsActive => CurrentState != ActionState.Inactive;
    }
}
