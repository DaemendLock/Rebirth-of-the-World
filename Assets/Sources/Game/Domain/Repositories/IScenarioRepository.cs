using Game.Domain.Entities;

using Lobby.Common.Primitives;

namespace Game.Domain.Repositories
{
    public interface IScenarioRepository
    {
        void Add(Scenario scenario);
        void Remove(ScenarioId id);
        Scenario Get(ScenarioId id);
    }
}
