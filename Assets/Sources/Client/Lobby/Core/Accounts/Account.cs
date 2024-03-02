using Client.Lobby.Core.Common;
using Client.Lobby.Core.Loading;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Utils.DataTypes;

namespace Client.Lobby.Core.Accounts
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class AccoutnProgression
    {
        public ProgressValue Level { get; set; }
    }

    public class CharacterData
    {
        private int _activeSpec;
        private ProgressValue _level;
        private ProgressValue _affection;
        private ItemId[] _items;

        private DelaiedData<CharacterData> _delaiedData;

        public CharacterData(int id, bool avaible, byte activeViewSet, DelaiedData<CharacterData> data)
        {
            Id = id;
            ActiveViewSet = activeViewSet;
            _delaiedData = data;
            IsAvaible = avaible;
        }

        public CharacterData(int id, bool avaible, byte activeViewSet, int activeSpec, ProgressValue level, ProgressValue affection, ItemId[] items)
        {
            Id = id;
            ActiveViewSet = activeViewSet;
            _activeSpec = activeSpec;
            _level = level;
            _affection = affection;
            _items = items;
            IsAvaible = avaible;
        }

        public int Id { get; }

        public byte ActiveViewSet { get; set; }

        public bool IsAvaible { get; set; }

        public ProgressValue Level
        {
            get
            {
                if (_delaiedData == null)
                {
                    return _level;
                }

                return _delaiedData.GetValue()._level;
            }

            set => _level = value;
        }

        public ProgressValue Affection
        {
            get
            {
                if (_delaiedData == null)
                {
                    return _affection;
                }

                return _delaiedData.GetValue()._affection;
            }
            set => _affection = value;
        }

        public ItemId[] Items
        {
            get => _items ??= _delaiedData.GetValue()._items;
            set => _items = value;
        }

        public int ActiveSpec
        {
            get
            {
                if (_delaiedData == null)
                {
                    return _activeSpec;
                }

                return _delaiedData.GetValue()._activeSpec;
            }

            set => _activeSpec = value;
        }
    }

    public class Account : UpdateableModel
    {
        public event Action Updated;

        private readonly AccountInfo _info;
        private DelaiedData<Account> _delaiedData;
        private AccoutnProgression _progression;
        private List<CharacterData> _characterDatas = new();

        public Account(AccountInfo info, AccoutnProgression progression, List<CharacterData> characterDatas)
        {
            _info = info;
            _progression = progression;
            _characterDatas = characterDatas;
        }

        public AccountInfo Info => _info;

        public AccoutnProgression Progression
        {
            get
            {
                _progression ??= _delaiedData.GetValue()._progression;

                return _progression;
            }
        }

        public CharacterData GetCharacterData(int characterId)
        {
            return _characterDatas.Find(data => data.Id == characterId);
        }

        public bool GetCharacterAvaible(int characterId)
        {
            return true;
        }
    }
}
