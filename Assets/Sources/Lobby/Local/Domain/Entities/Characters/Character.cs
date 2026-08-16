using Lobby.Common.Primitives;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.Entities.Characters
{
    public ref struct Character
    {
        public Character(CharacterKey id, Level level, bool isAvailable)
        {
            Id = id;
            Level = level;
            IsAvailable = isAvailable;
        }

        public CharacterKey Id { get; }
        public Level Level { get; set; }
        public bool IsAvailable { get; set; }
    }
}
