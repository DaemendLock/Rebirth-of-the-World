using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Data.Models
{
    public sealed class AccountData
    {
        public AccountData(Account account)
        {
            Name = account.Name;
            Title = account.Title;
            Level = account.Level;
            AvatarCharacterId = account.AvatarCharacterId;
            CurrentEncounter = account.CurrentEncounter;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public CharacterKey? AvatarCharacterId { get; set; }
        public EncounterId? CurrentEncounter { get; set; }

        public void UpdateFrom(Account account)
        {
            CurrentEncounter = account.CurrentEncounter;
            Name = account.Name;
            Title = account.Title;
            Level = account.Level;
            AvatarCharacterId = account.AvatarCharacterId;
        }

        public Account ToAccount(AccountId accountId)
        {
            return new(accountId)
            {
                Name = Name,
                Title = Title,
                Level = Level,
                AvatarCharacterId = AvatarCharacterId,
                CurrentEncounter = CurrentEncounter
            };
        }
    }
}
