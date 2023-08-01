using UnityEngine;
using UnityEngine.UI;

namespace Remaster.View.UIElements
{
    [RequireComponent(typeof(Slider))]
    public class Healthbar : MonoBehaviour
    {
        private Slider _bar;

        private void Start()
        {
            _bar = GetComponent<Slider>();
        }

        public void OnHealthChanged(float currentHealth, float maxHealth)
        {
            _bar.maxValue = maxHealth;
            _bar.value = currentHealth;
        }
    }
}
