using Combat.Common.Primitives;

using Lobby.Common.Primitives;

namespace Lobby.Local.Application.DTO
{
    public readonly struct ScenarioSelectCharacterRequest
    {
        public ScenarioSelectCharacterRequest(AccountId requestedBy, CharacterSelectInfo? characterPickInfo)
        {
            RequestedBy = requestedBy;
            CharacterPickInfo = characterPickInfo;
        }

        public AccountId RequestedBy { get; }
        public CharacterSelectInfo? CharacterPickInfo { get; }
    }

    public readonly struct CharacterSelectInfo
    {
        public CharacterSelectInfo(CharacterKey characterKey)
        {
            CharacterKey = characterKey;
        }

        public CharacterKey CharacterKey { get; }
    }
}
