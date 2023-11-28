using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.Gallery.Widgets
{
    internal class FilesContainerWidget : MonoBehaviour
    {
        private List<CharacterCardWidget> _cards;

        public void AddCard(CharacterCardWidget card)
        {
            _cards.Add(card);
        }
    }
}
