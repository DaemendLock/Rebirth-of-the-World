using UnityEngine;

namespace Client.Lobby.View.Utils
{
    public class StaticSprite : SpriteProvider
    {
        private readonly Sprite _sprite;

        public StaticSprite(Sprite sprite)
        {
            _sprite = sprite;
        }

        public Sprite GetSprite() => _sprite;
    }
}
