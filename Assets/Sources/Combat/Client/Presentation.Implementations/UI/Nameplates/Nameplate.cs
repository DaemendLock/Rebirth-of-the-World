using UnityEngine;

using Client.Combat.Presentation.UI.Elements;
using Client.Combat.Presentation.UI.ResourceBar;
using Client.Combat.Domain.Units;

namespace Client.Combat.Presentation.Units
{
    public class Nameplate : BindableViewComponent<IUnit>
    {
        [SerializeField] private SelectionFrame _selectionFrame;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ResourceBar _resources;
        [SerializeField] private CastBar _castbar;

        [SerializeField] private Color _friendlyColor;
        [SerializeField] private Color _enemyColor;

        //protected override void OnCastChanged() => _castbar.ShowCast(Model.ActiveCast);
        //protected override void OnHealthChanged() => _healthBar.ModifyHealth(Model.Health);
        //protected override void OnPositionChanged() { /*TODO*/ }
}
}