using Client.Combat.Domain.Units.Components;

using UnityEngine;

namespace Client.Combat.Presentation.UI.Elements
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
    }
}
