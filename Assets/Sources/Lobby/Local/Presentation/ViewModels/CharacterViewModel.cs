using Lobby.Common.Primitives;
using Lobby.Local.Domain.ValueObjects;

using UnityEngine;

namespace Lobby.Local.Presentation.ViewModels
{
    public sealed class CharacterViewModel
    {
        public CharacterKey CharacterKey { get; set; }
        public string LocalizedName { get; set; }
        public Sprite CardPhoto { get; set; }
        public Level Level { get; set; }
    }
}
