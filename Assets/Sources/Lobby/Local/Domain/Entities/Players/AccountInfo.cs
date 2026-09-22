using Combat.Common.Primitives;

using Lobby.Common.Primitives;

namespace Lobby.Local.Domain.Entities
{
    public ref struct AccountInfo
    {
        public AccountInfo(AccountId accountId)
        {
            Id = accountId;
            Name = "Player" + accountId.ToString();
            Title = "No Title";
            Level = 1;
            AvatarCharacterId = null;
        }
        public AccountId Id { get; }

        public string Name { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public CharacterKey? AvatarCharacterId { get; set; }
    }
}
