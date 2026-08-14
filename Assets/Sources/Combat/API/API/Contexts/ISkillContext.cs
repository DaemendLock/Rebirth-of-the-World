using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.API.Contexts
{
    public readonly struct GameEvent<TEventData> where TEventData : IEventData
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

        EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData;
        void Unsubscribe(EventHandlerId id);

        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
