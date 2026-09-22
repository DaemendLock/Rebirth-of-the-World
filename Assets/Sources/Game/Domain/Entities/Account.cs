using Lobby.Common.Primitives;

namespace Game.Domain.Entities
{
    public sealed class Account
    {
        public AccountId Id { get; }

        public string Name { get; private set; }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new System.InvalidOperationException();
            }

            Name = newName;
        }
    }
}
