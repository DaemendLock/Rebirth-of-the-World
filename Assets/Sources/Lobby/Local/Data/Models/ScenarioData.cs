using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Data.Models
{
    public sealed class ScenarioData
    {
        public readonly PlayerCharacterSelection?[] Characters;

        public ScenarioData(Scenario scenario)
        {
            Name = scenario.Name;
            LocationName = scenario.LocationName;
            Characters = scenario.SelectedCharacters.ToArray();
        }

        public string Name { get; set; }
        public string LocationName { get; set; }

        public void UpdateFrom(Scenario scenario)
        {
            scenario.SelectedCharacters.CopyTo(Characters);
            Name = scenario.Name;
            LocationName = scenario.LocationName;
        }

        public Scenario ToScenario(ScenarioId id)
        {
            return new()
            {
                Id = id,
                Name = Name,
                LocationName = LocationName,
                SelectedCharacters = Characters
            };
        }
    }
}
