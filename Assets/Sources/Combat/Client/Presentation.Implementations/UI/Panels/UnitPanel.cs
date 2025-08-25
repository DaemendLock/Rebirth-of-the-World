using UnityEngine;
using UnityEngine.UI;

using Client.Combat.Presentation.UI.Elements;

namespace Client.Combat.Presentation.UI.Panels
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class UnitPanel : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CastBar _castBar;

        //protected override void OnCastChanged() => _castBar.ShowCast(Model.ActiveCast);
        //protected override void OnHealthChanged() => _healthBar.ModifyHealth(Model.Health);
        //protected override void OnPositionChanged() { }
    }
}
