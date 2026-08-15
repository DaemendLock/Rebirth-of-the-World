using Combat.API.Events;
using Combat.API.Skills;
using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface ISkillContext
    {
        void StartCooldown(float value);

        SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData;
        void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData;

        EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData;
        void Unsubscribe(EventHandlerId id);

        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
