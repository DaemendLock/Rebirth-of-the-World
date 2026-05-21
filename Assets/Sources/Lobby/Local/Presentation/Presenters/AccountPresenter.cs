using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Accounts;

namespace Lobby.Local.Presentation.Presenters
{
    public sealed class AccountPresenter : IAccountCreateOutput
    {
        void IAccountCreateOutput.Present(Account account) => UnityEngine.Debug.Log($"Created account: Id - {account.Id}; Name - {account.Name}");
    }
}
