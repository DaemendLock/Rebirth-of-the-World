using Combat.Common.Primitives;

using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Data.Models
{
    public sealed class AccountData
    {
        public AccountData(AccountInfo account)
        {
            Name = account.Name;
            Title = account.Title;
            Level = account.Level;
            AvatarCharacterId = account.AvatarCharacterId;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public CharacterKey? AvatarCharacterId { get; set; }

        public AccountInfo ToAccount(AccountId accountId)
        {
            return new(accountId)
            {
                Name = Name,
                Title = Title,
                Level = Level,
                AvatarCharacterId = AvatarCharacterId
            };
        }
    }
}
