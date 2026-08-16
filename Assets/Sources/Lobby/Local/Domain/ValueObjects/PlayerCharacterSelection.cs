using Lobby.Common.Primitives;

namespace Lobby.Local.Domain.ValueObjects
{
    public readonly struct PlayerCharacterSelection
    {
        public readonly AccountId Player;
        public readonly CharacterKey? CharacterKey;

        public PlayerCharacterSelection(AccountId player, CharacterKey? characterId)
        {
            Player = player;
            CharacterKey = characterId;
        }
    }
}
