using Lobby.Common.Primitives;

namespace Game.Application.DTO
{
    public readonly struct ScenarioInfo
    {
        public ScenarioInfo(ScenarioId id, string name)
        {
            Id = id;
            Name = name;
        }

        public ScenarioId Id { get; }
        public string Name { get; }
    }
}
