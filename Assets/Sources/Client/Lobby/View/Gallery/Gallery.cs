using Client.Lobby.Core.Characters;
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

        public CharacterCardWidget FindCharacterCard(Character character) => _filesContainer.Find(character);

        public void AddCard(CharacterCardWidget card)
        {
            _filesContainer.AddCard(card);
        }

        public void SetActive(bool active) => gameObject.SetActive(active);

        protected override void OnModelUpdate()
        {
            
        }
    }
}