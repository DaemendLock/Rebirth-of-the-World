using Game.Application.Repositories;
using Game.Domain.Entities;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

using System.Collections.Generic;
using System.Linq;

namespace Lobby.Local.Gateways.Repositories
{
    public sealed class LocalScenarioRepository : IScenarioRepository, IScenarioQueryGateway
    {
        private readonly Dictionary<ScenarioId, Scenario> _values = new();

        public void Add(Scenario scenario) => _values.Add(scenario.Id, scenario);

        public void Remove(ScenarioId id) => _values.Remove(id);

        public Scenario Get(ScenarioId id) => _values[id];

        public ScenarioId? GetScenarioByAccount(AccountId accountId) =>
            _values.Values.FirstOrDefault(value => value._members.Any(
                member => member.HasValue && member.Value.AccountId == accountId))?.Id;
    }
}
