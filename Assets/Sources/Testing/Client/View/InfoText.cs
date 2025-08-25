using TMPro;

using UnityEngine;

namespace Client.Testing.View
{
    public class InfoText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textElement;
        private string _message;

        private void Awake()
        {
            _message = _textElement.text;
        }

        public KeyCode KeyCode
        {
            set => _textElement.text = _message.Replace("$Key", value.ToString());
        }
    }
}
