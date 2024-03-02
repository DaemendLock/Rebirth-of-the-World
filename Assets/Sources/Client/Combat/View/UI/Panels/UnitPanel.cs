using Client.Combat.Core.Units.Components;
using Client.Combat.View.UI.Elements;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace Client.Combat.View.UI.Panels
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class UnitPanel : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CastBar _castBar;

        public void ShowCast(SpellCast cast) => _castBar.ShowCast(cast);

        public void UpdateHealth(Health health) => _healthBar.SetValue(health);
    }
}
