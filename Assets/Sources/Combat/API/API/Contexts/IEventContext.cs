namespace Combat.API.Contexts
{
    public interface IEventContext
    {
        delegate void EventHandler<T>(GameEvent<T> @event) where T : unmanaged, IEventData;

        void SubscribeToEvent<T>(EventHandler<T> callback) where T : unmanaged, IEventData;
        void DoSomething<T>(GameEvent<T> @event) where T : unmanaged, IEventData; //TODO: rename
    }
}
