using Utils.DataTypes;

namespace Client.Lobby.Domain.Accounts
{
    public class CharacterData
    {
        private readonly bool _isFull;
        private readonly int _accountId;
        private readonly int _activeSpec;
        private readonly ProgressValue _level;
        private readonly ProgressValue _affection;
        private readonly ItemId[] _items;

        public CharacterData(int accountId, int id, byte activeViewSet)
        {
            _accountId = accountId;
            CharacterId = id;
            ActiveViewSet = activeViewSet;
            _isFull = false;
        }

        public CharacterData(int accountId, int id, byte activeViewSet, int activeSpec, ProgressValue level, ProgressValue affection, ItemId[] items)
        {
            _accountId = accountId;
            CharacterId = id;
            ActiveViewSet = activeViewSet;
            _activeSpec = activeSpec;
            _level = level;
            _affection = affection;
            _items = items;
            _isFull = true;
        }

        public int CharacterId { get; }

        public int AccountId => _accountId;

        public byte ActiveViewSet { get; }

        public bool Full => _isFull;

        public ProgressValue Level
        {
            get => _level;
        }

        public ProgressValue Affection
        {
            get => _affection;
        }

        public ItemId[] Items
        {
            get => _items;
        }

        public int ActiveSpec => _activeSpec;
    }
}
