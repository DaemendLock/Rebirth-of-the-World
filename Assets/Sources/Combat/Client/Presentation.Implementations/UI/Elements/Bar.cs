using UnityEngine;
using UnityEngine.UI;

namespace Client.Combat.Presentation.UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class Bar : MonoBehaviour
    {
        [SerializeField] private Image _filler;

        private Image _background;

        private void Start()
        {
            _background = GetComponent<Image>();

            if (_filler == null)
            {
                throw new System.ArgumentNullException($"{nameof(_filler)} in {gameObject.name}");
            }
        }

        public float MaxValue { get; set; }

        public float Value { set => _filler.fillAmount = value / MaxValue; }

        public Color Color { get => _filler.color; set => _filler.color = value; }
    }
}
