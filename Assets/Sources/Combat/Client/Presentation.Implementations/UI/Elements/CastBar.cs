using Client.Combat.Domain.Units.Components;

using UnityEngine;

namespace Client.Combat.Presentation.UI.Elements
{
    public class CastBar : MonoBehaviour
    {
        [SerializeField] private Bar _bar;

        //private SpellCasting _activeCast;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        //private void Update()
        //{
        //    _bar.Value = _activeCast.ActiveCast.Duration.Left;
        //}

        //public void ShowCast(SpellCasting cast)
        //{
        //    if (cast == null)
        //    {
        //        gameObject.SetActive(false);
        //    }

        //    _activeCast = cast;
        //    gameObject.SetActive(true);
        //    _bar.MaxValue = cast.ActiveCast.Duration.FullTime;
        //}
    }
}
