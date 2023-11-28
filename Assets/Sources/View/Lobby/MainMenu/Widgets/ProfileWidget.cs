using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.Lobby.MainMenu.Widgets
{
    internal class ProfileWidget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _lvlText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _titleText;

        public string Name
        {
            get => _nameText.text;
            set => _nameText.text = value;
        }

        public string Title
        {
            get => _titleText.text;
            set => _titleText.text = value;
        }

        public string Level
        {
            get => _lvlText.text;
            set => _lvlText.text = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Show profile");
        }
    }
}
