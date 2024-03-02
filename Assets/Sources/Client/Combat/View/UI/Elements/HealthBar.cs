using Client.Combat.Core.Units.Components;
using UnityEngine;

namespace Client.Combat.View.UI.Elements
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Bar _health;
        [SerializeField] private Bar _absorption;

        public Color Color
        {
            get => _health.Color;
            set => _health.Color = value;
        }

        public void SetValue(Health value)
        {
            _health.MaxValue = value.MaxHealth;
            _health.Value = value.CurrentHealth;
            _absorption.MaxValue = value.MaxHealth;
            _absorption.Value = value.Absorption;
        }
    }
}
