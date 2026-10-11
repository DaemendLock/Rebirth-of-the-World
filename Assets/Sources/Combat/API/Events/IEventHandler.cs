namespace Combat.API.Events
{
    public interface IEventHandler<T> where T : IEventData
    {
        void Handle(GameEvent<T> @event);
    }
}
