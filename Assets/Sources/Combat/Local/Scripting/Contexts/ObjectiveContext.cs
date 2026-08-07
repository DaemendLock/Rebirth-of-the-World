using Combat.API;
using Combat.API.API.Tasks;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class ObjectiveContext : IObjectiveContext
    {
        public EncounterApi Encounter => throw new System.NotImplementedException();

        public TQuery GetCapability<TQuery>() where TQuery : class => throw new System.NotImplementedException();
        public T GetData<T>() where T : unmanaged, IObjectiveData => throw new System.NotImplementedException();
    }
}
