using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;

using System.Collections.Generic;

namespace Lobby.Local.Domain.Repositories
{
    public interface IScenarioRepository
    {
        void Create(Scenario scenario);
        Scenario Get(ScenarioId id);
        void Update(Scenario scenario);
        void Delete(ScenarioId id);

        IReadOnlyCollection<ScenarioId> GetAllIds();
    }
}
