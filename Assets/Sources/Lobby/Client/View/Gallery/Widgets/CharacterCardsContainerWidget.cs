using Client.Lobby.View.Gallery.Widgets;

using UnityEngine;

namespace View.Lobby.Gallery.Widgets
{
    internal class CharacterCardsContainerWidget : MonoBehaviour
    {
        public void AddCard(CharacterCardWidget card)
        {
            card.transform.SetParent(transform, false);
        }
    }
}
