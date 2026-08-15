using Combat.API.Skills;
using Combat.API.Statuses;

namespace Combat.API.Contexts
{
    public interface IStatusContext
    {
        TQuery GetCapability<TQuery>() where TQuery : class;
        StatusState<T> GetState<T>() where T : unmanaged, IDynamicStatusData;
        void SaveState<T>(StatusState<T> value) where T : unmanaged, IDynamicStatusData;
    }
}
