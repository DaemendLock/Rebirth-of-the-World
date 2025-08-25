namespace Server.Combat.Domain.Events
{
    public interface IEvent
    {
        bool InProgress { get; }

        void Cancel();
    }
}
