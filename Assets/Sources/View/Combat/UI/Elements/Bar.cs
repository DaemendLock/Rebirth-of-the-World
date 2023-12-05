using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace View.Combat.UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class Bar : MonoBehaviour
    {
        //TODO: move???
        private List<Color> _resourceColors = new List<Color>() { Color.white, Color.blue, Color.yellow, Color.green, Color.white, Color.white, Color.black };

        [SerializeField] private Image _filler;
        private Image _background;

        public float MaxValue
        {
            get;
            set;
        }

        public float Value { set => _filler.fillAmount = value / MaxValue; }

        public void SetResourceType(ResourceType resource)
        {
            _filler.color = _resourceColors[(int) resource];
        }

        private void Start()
        {
            _background = GetComponent<Image>();
        }
    }
}
