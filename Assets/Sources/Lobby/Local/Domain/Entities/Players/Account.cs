using Lobby.Common.Primitives;

namespace Lobby.Local.Domain.Entities
{
    public ref struct PlayerProfile
    {
        public AccountId AccountId { get; }
        public string Name { get; set; }
        public AccountTiltleId TiltleId { get; set; }
    }

    public ref struct Account
    {
        public AccountId Id { get; }

        public Account(AccountId accountId)
        {
            Id = accountId;
            Name = "Player" + accountId.ToString();
            Title = "No Title";
            Level = 1;
            AvatarCharacterId = null;
            CurrentEncounter = default;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public CharacterKey? AvatarCharacterId { get; set; }

        public EncounterId? CurrentEncounter { get; set; }
    }
}
