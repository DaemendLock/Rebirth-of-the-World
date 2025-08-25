namespace Server.Combat.Domain.Events
{
    public struct HealthChangeEvent : IEvent
    {
        public bool InProgress { get; private set; }

        public void Cancel() => InProgress = false;
    }
}
