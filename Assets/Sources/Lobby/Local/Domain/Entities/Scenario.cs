using Lobby.Common.Primitives;
using Lobby.Local.Domain.ValueObjects;

using System;

namespace Lobby.Local.Domain.Entities
{
    public ref struct Scenario
    {
        public ScenarioId Id { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }

        public Span<PlayerCharacterSelection?> SelectedCharacters { get; set; }

        public int PlayerCount => SelectedCharacters.Length;
    }
}
