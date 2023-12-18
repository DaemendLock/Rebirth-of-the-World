using Core.Lobby.Accounts;
using Core.Lobby.Characters;
using Data.Characters;
using UnityEngine;
using View.Lobby.Gallery.Widgets;
using View.Lobby.Utils;

public enum GallerySorting : int
{
    Id = 0,
    Name = 1,
    Level = 2,
    Affection = 3
}

public enum GalleryFilter : int
{
    All = 0,
    Tank = 1,
    Healer = 2,
    Support = 3,
    Dps = 4,
}

namespace View.Lobby.Gallery
{
    public class Gallery : MenuElement
    {
        //private AccountData accountData;
        [SerializeField] private FilesContainerWidget _filesContainer;
        [SerializeField] private CharacterCardWidget _prefab;

        private void OnEnable()
        {
            Lobby.Instance.CharacterSheetMode = CharacterSheet.ViewMode.Edit;
            UpdateCards();
        }

        private void UpdateCards()
        {
            int[] ids = Character.GetLoadedCharactersId();

            foreach (int id in ids)
            {
                if (_filesContainer.ContainsCard(id))
                {
                    continue;
                }

                CreateCharacterFile(Character.Get(id));
            }
        }

        private void CreateCharacterFile(Character character)
        {
            CharacterCardWidget card = Object.Instantiate(_prefab, _filesContainer.transform);
            
            card.Init(character, AccountsDataProvider.GetCharacterData(character.Id, AccountsDataProvider.ActiveAccount));

            _filesContainer.AddCard(card);
        }
    }
}