using Client.Lobby.Core.Characters;
using Client.Lobby.View.Gallery.Widgets;
using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.Gallery.Widgets
{
    internal class CharacterCardsContainerWidget : MonoBehaviour
    {
        private List<CharacterCardWidget> _cards = new();

        public void AddCard(CharacterCardWidget card)
        {
            card.transform.SetParent(transform, false);
            _cards.Add(card);
        }

        public CharacterCardWidget Find(Character character) => _cards.Find((value) => value.CharacterId == character.Info.Id);
    }
}
