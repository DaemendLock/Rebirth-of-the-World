using Lobby.Common.Primitives;

namespace Lobby.Local.Domain.ValueObjects
{
    public readonly struct PlayerCharacterSelection
    {
        public readonly AccountId Player;
        public readonly CharacterId? CharacterId;

        public PlayerCharacterSelection(AccountId player, CharacterId? characterId)
        {
            Player = player;
            CharacterId = characterId;
        }
    }
}
