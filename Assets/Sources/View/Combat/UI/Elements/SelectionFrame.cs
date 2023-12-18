using UnityEngine;
using UnityEngine.UI;

namespace View.Combat.UI.Elements
{
    internal class SelectionFrame : MonoBehaviour
    {
        [SerializeField] private Image _frame;
        [SerializeField] private float _frameScale = 1;

        public float FrameScale => _frameScale;

        public bool Selected
        {
            get => _frame.enabled;
            set => _frame.enabled = value;
        }
    }
}
