using System.Collections.Generic;

using Client.Lobby.Domain.Accounts;
using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Common.CoreViews;
using Client.Lobby.View.Gallery.Widgets;
using Client.Lobby.View.Utils;

using UnityEngine;

using View.Lobby.Gallery.Widgets;

namespace Client.Lobby.View.Gallery
{
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

    public class Gallery : BindableView<CharacterGallery>, IMenuElement
    {
        [SerializeField] private CharacterCardsContainerWidget _filesContainer;

        private readonly List<CharacterCardWidget> _cards = new();

        public CharacterCardWidget FindCharacterCard(Character character) => _cards.Find(card => card.CharacterId == character.Info.Id);

        public CharacterCardWidget WidgetAt(int index) => _cards[index];

        public void AddCard(CharacterCardWidget card)
        {
            _filesContainer.AddCard(card);
            _cards.Add(card);
        }

        public void SetActive(bool active) => gameObject.SetActive(active);

        protected override void OnModelUpdate()
        {
            foreach (CharacterCardWidget characterCardWidget in _cards)
            {
                if (Model.TryGetCharacter(characterCardWidget.CharacterId, out Character character))
                {
                    if (character == characterCardWidget.Model)
                    {
                        continue;
                    }

                    characterCardWidget.Bind(character);
                    characterCardWidget.SetAvaible(true);
                }
                else
                {
                    characterCardWidget.SetAvaible(false);
                }
            }
        }
    }
}