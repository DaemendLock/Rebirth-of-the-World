using UnityEngine;

using Client.Lobby.Domain.Accounts;
using Client.Lobby.Infrastructure.Controllers;

using UtilsUnity.ThrowHepler;
using Utils.ThrowHepler;

namespace Client.Lobby.Infrastructure.Networking
{
    public interface ServerCommand
    {
        void Perform();
    }

    public enum ServerCommandType : byte
    {
        PrintDebug,
        SetAccountInfo,
    }

    public class PrintMessageCommand : ServerCommand
    {
        private string _message;

        public PrintMessageCommand(string message)
        {
            _message = message;
        }

        public void Perform() => Debug.Log(_message);
    }

    public class SetAccountData : ServerCommand
    {
        private readonly Account _account;
        private readonly AccountDataType _type;
        private readonly AccountsController _accountsController;

        public SetAccountData(Account account, AccountDataType type, AccountsController accountsController)
        {
            ThrowHepler.ArgumentNullException(account);

            _account = account;
            _type = type;
            _accountsController = accountsController;
        }

        public void Perform()
        {
            int id = _account.Id;
            Account account = _accountsController.GetAccount(id);
            account.Update(_type, _account);
        }
    }
}
