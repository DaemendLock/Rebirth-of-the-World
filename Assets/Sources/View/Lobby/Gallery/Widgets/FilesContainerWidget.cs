using Data.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.Gallery.Widgets
{
    internal class FilesContainerWidget : MonoBehaviour
    {
        private List<CharacterCardWidget> _cards = new();

        internal bool ContainsCard(int id) => _cards.Find((value) => value.CharacterId == id) != null;

        public void AddCard(CharacterCardWidget card)
        {
            _cards.Add(card);
        }
    }
}
