using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public enum InterruptReason
    {
        None,
        Chained,
        Death,
        Forced
    }

    public sealed class Action
    {
        private readonly IActionStrategy _strategy;

        public Action(ActionId id, SkillId source, ActionFlags flags, IActionStrategy strategy)
        {
            Id = id;
            Source = source;
            Flags = flags;
            _strategy = strategy;

            ActiveTime = 0;
        }

        public ActionId Id { get; }

        public SkillId Source { get; }

        public ActionFlags Flags { get; }

        public IActionStrategy Strategy => _strategy;

        public float ActiveTime { get; private set; }

        public ActionState CurrentState => _strategy.State;

        public bool CanChainInto(SkillId skillId) => _strategy.CanChainInto(skillId);

        public void Start()
        {
            _strategy.Start();
        }

        public void Progress(float deltaTime)
        {
            ActiveTime += deltaTime;
            _strategy.Progress(deltaTime);
        }

        public void Interrupt(InterruptReason reason)
        {
            _strategy.Interrupt();
        }

        public bool AllowMovement => Flags.HasFlag(ActionFlags.AllowMovement);
        public bool CanInterrupt => Flags.HasFlag(ActionFlags.CanInterrupt);
        public bool IsActive => CurrentState != ActionState.Inactive;
    }
}
