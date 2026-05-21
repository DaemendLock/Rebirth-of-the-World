using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;

namespace Lobby.Local.Data.Models
{
    public sealed class AccountData
    {
        public AccountData(Account account)
        {
            Name = account.Name;
            CurrentEncounter = account.CurrentEncounter;
        }

        public string Name { get; set; }
        public EncounterId? CurrentEncounter { get; set; }

        public void UpdateFrom(Account account)
        {
            CurrentEncounter = account.CurrentEncounter;
            Name = account.Name;
        }

        public Account ToAccount(AccountId accountId)
        {
            return new(accountId)
            {
                Name = Name,
                CurrentEncounter = CurrentEncounter
            };
        }
    }
}
