using Game.Domain.Entities;
using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System.Linq;

namespace Lobby.Local.Gateways.Models
{
    public sealed class ScenarioData
    {
        public readonly ScenarioMemberInfo?[] Members;

        public ScenarioData(Scenario scenario)
        {
            Name = scenario.Name;
            LocationName = scenario.LocationName;
            Members = scenario._members.ToArray();
        }

        public string Name { get; set; }
        public string LocationName { get; set; }

        public void UpdateFrom(Scenario scenario)
        {
            scenario._members.CopyTo(Members, 0);
            Name = scenario.Name;
            LocationName = scenario.LocationName;
        }

        public Scenario ToScenario(ScenarioId id)
        {
            return new(id, LocationName, Members)
            {
                Name = Name
            };
        }
    }
}
