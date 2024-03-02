using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace Client.Combat.View.UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class Bar : MonoBehaviour
    {
        //TODO: move???
        private List<Color> _resourceColors = new List<Color>() { Color.white, Color.blue, Color.yellow, Color.green, Color.white, Color.white, Color.black };

        [SerializeField] private Image _filler;
        private Image _background;

        private void Start()
        {
            _background = GetComponent<Image>();

            if(_filler == null)
            {
                throw new System.ArgumentNullException(nameof(_filler));
            }
        }

        public float MaxValue
        {
            get;
            set;
        }

        public float Value { set => _filler.fillAmount = value / MaxValue; }

        public Color Color { get => _filler.color; set => _filler.color = value; }

        public void SetResourceType(ResourceType resource)
        {
            _filler.color = _resourceColors[(int) resource];
        }
    }
}
