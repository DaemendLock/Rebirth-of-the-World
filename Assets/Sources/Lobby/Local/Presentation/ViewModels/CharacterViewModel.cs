using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Presentation.ViewModels
{
    public sealed class CharacterViewModel
    {
        public string LocalizedName { get; set; }
        public Level Level { get; set; }
    }
}
