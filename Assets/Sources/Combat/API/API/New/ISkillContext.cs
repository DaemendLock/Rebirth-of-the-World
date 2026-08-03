using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.API.API.Skills
{
    public readonly struct GameEvent<TEventData> where TEventData : unmanaged, IEventData
    {

    }

    public interface IEventData { }

    public interface IEnvironmentContext
    {
        bool TryGetTarget(UnitId id, out ITargetable targetable);
    }

    public interface ISkillContext
    {
        SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData;
        void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData;

        void SubscribeToEvent<TEventData>(System.Action<GameEvent<TEventData>> callback) where TEventData : unmanaged, IEventData;
        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
