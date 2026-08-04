using Combat.API.Skills;

namespace Combat.API.Contexts
{
    public readonly struct GameEvent<TEventData> where TEventData : unmanaged, IEventData
    {
        public readonly TEventData Data;

        public GameEvent(TEventData data)
        {
            Data = data;
        }
    }

    public interface IEventData { }

    public interface ISkillContext
    {
        SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData;
        void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData;

        void SubscribeToEvent<TEventData>(System.Action<GameEvent<TEventData>> callback) where TEventData : unmanaged, IEventData;
        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
