using System;
using System.Threading.Tasks;

using Client.Lobby.Domain.Characters;
using Client.Lobby.Domain.Common;

using Utils.DataTypes;
using Utils.Patterns.DataProviders;

namespace Client.Lobby.Domain.Accounts
{
    [Flags]
    public enum AccountDataType : byte
    {
        Empty,
        Name = 1,
        Title = 2,
        Level = 4,
        CharacterData = 8,
    }

    public class Account : IUpdateableModel
    {
        public event Action Updated;
        
        private readonly AsyncDataProvider<AccountDataType, Account> _asyncDataProvider;

        private string _name;
        private ProgressValue? _level;

        public Account(int id, AsyncDataProvider<AccountDataType, Account> asyncDataProvider, AsyncDataProvider<int, Character> charactersDataProvider)
        {
            Id = id;
            Characters = new(charactersDataProvider);

            _asyncDataProvider = asyncDataProvider;
        }

        public int Id { get; }
        public CharacterGallery Characters { get; }

        public async Task<string> GetName() => _name ??= (await _asyncDataProvider.GetValue(AccountDataType.Name))._name;
        public void SetName(string name) => _name = name;

        public async Task<ProgressValue> GetLevel() => (_level ??= (await _asyncDataProvider.GetValue(AccountDataType.Level))._level).Value;
        public void SetLevel(ProgressValue level) => _level = level;

        public void Update(AccountDataType dataType, Account data)
        {
            if (dataType.HasFlag(AccountDataType.Name))
            {
                _asyncDataProvider.ProvideData(AccountDataType.Name, data);
            }

            if (dataType.HasFlag(AccountDataType.Title))
            {
                _asyncDataProvider.ProvideData(AccountDataType.Title, data);
            }

            if (dataType.HasFlag(AccountDataType.Level))
            {
                _asyncDataProvider.ProvideData(AccountDataType.Level, data);
            }

            if (dataType.HasFlag(AccountDataType.CharacterData))
            {
                Characters.Update(data.Characters);
            }

            if (dataType != AccountDataType.Empty && dataType != AccountDataType.CharacterData)
            {
                Updated?.Invoke();
            }
        }
    }
}
