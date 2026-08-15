namespace Combat.API.Events
{
    public interface IEventData { }

    public readonly struct GameEvent<TEventData> where TEventData : IEventData
    {
        public readonly TEventData Data;

        public GameEvent(TEventData data)
        {
            Data = data;
        }
    }
}
