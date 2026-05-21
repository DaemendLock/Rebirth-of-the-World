using UnityEngine;
using UnityEngine.UI;

namespace Combat.View.Common
{
    public enum FillDirection
    {
        None, BottomToTop, TopToBottom, LeftToRight, RightToLeft
    }

    public sealed class Bar : MonoBehaviour
    {
        [SerializeField] private Image _filler;
        [SerializeField] private Image _background;

        public float FillPercent
        {
            get => _filler.fillAmount;
            set
            {
                _filler.fillAmount = value;
            }
        }

        public Color BackgroundColor { get => _background.color; set => _background.color = value; }

        public Color FillerColor { get => _filler.color; set => _filler.color = value; }

        public FillDirection FillDirection
        {
            set
            {
                switch (value)
                {
                    case FillDirection.BottomToTop:
                        _filler.fillMethod = Image.FillMethod.Vertical;
                        _filler.fillOrigin = 0;
                        return;
                    case FillDirection.TopToBottom:
                        _filler.fillMethod = Image.FillMethod.Vertical;
                        _filler.fillOrigin = 1;
                        return;
                    case FillDirection.LeftToRight:
                        _filler.fillMethod = Image.FillMethod.Horizontal;
                        _filler.fillOrigin = 0;
                        return;
                    case FillDirection.RightToLeft:
                        _filler.fillMethod = Image.FillMethod.Horizontal;
                        _filler.fillOrigin = 1;
                        return;
                    default:
                        throw new System.InvalidOperationException();
                }
            }
        }
    }
}