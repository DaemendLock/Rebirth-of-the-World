using UnityEngine;

namespace Lobby.Local.Presentation.ViewModels.MainMenu
{
    public sealed class ProfileViewModel
    {
        public string Name { get; set; }
        public string LocalizedTitle { get; set; }
        public int Level { get; set; }
        public Sprite Avatar { get; set; }
    }
}
