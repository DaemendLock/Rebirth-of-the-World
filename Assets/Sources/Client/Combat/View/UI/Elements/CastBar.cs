using Client.Combat.Core.Units.Components;
using UnityEngine;

namespace Client.Combat.View.UI.Elements
{
    public class CastBar : MonoBehaviour
    {
        [SerializeField] private Bar _bar;

        private SpellCast _activeCast;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _bar.Value = _activeCast.Duration.Left;
        }

        public void ShowCast(SpellCast cast)
        {
            if (cast == null)
            {
                HideCast();
            }

            gameObject.SetActive(true);
            _bar.MaxValue = cast.Duration.FullTime;
        }

        public void HideCast()
        {
            gameObject.SetActive(false);
        }
    }
}
