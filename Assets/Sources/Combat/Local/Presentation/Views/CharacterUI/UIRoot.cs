using UnityEngine;

namespace Combat.View.CharacterUI
{
    public sealed class UIRoot : MonoBehaviour
    {
        public HealthBar HealthBar { get; private set; }

        private void Start()
        {
            HealthBar = GetComponentInChildren<HealthBar>();
        }
    }
}