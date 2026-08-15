using Lobby.Common.Primitives;
using Lobby.Local.Data.Models;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Lobby.Local.Data.Repositories
{
    public sealed class LocalScenarioRepository : IScenarioRepository
    {
        private readonly Dictionary<ScenarioId, ScenarioData> _values;
        public LocalScenarioRepository()
        {
            _values = new();
        }

        public void Create(Scenario scenario)
        {
            _values[scenario.Id] = new(scenario);
        }

        public void Delete(ScenarioId id) => _values.Remove(id);

        public Scenario Get(ScenarioId id) => _values[id].ToScenario(id);

        public void Update(Scenario scenario)
        {
            _values[scenario.Id].UpdateFrom(scenario);
        }

        public IReadOnlyCollection<ScenarioId> GetAllIds() => _values.Keys;
    }
}
