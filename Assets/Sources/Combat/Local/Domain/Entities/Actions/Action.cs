using Combat.Common.Flags;
using Combat.Common.Primitives;

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

        public Action(ActionId id, ActionFlags flags, IActionStrategy strategy)
        {
            Id = id;
            Flags = flags;
            _strategy = strategy ?? throw new System.ArgumentNullException(nameof(strategy));

            ActiveTime = 0;
        }

        public ActionId Id { get; }

        public ActionFlags Flags { get; }

        public float ActiveTime { get; private set; }

        public bool IsComplete => _strategy.IsComplete;

        public bool AllowMovement => Flags.HasFlag(ActionFlags.AllowMovement);

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
            _strategy.Interrupt(reason);
        }

        public bool TryGet<T>(out T capability) where T : class
        {
            capability = _strategy as T;
            return capability != null;
        }
    }
}
