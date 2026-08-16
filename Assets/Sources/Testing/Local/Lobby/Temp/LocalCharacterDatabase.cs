using Lobby.Local.Data.DataSources;

using System;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.Temp
{
    [Serializable]
    public sealed class CharacteData
    {
        public string Id;
        public string Name;
        public Sprite Icon;
        public GameObject ModelPrefab;
    }

    public sealed class LocalCharacterDatabase : MonoBehaviour, ICharacterListDataSource
    {
        [SerializeField] private CharacteData[] _values;

        public CharacteData[] CharactersData => _values;
    }
}
