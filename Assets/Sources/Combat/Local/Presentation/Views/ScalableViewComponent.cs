using UnityEngine;

namespace Combat.Local.Presentation.Views
{
    [RequireComponent(typeof(CharacterViewComponent))]
    public sealed class ScalableViewComponent : MonoBehaviour
    {
        private CharacterViewComponent _characterModel;

        private void Start()
        {
            _characterModel = GetComponent<CharacterViewComponent>();
        }
        public float Rate { get; set; }

        private void Update()
        {
            float currentScale = transform.localScale.x;

            currentScale += Rate * Time.deltaTime * _characterModel.TimeScale;
            transform.localScale = Vector3.one * currentScale;
        }
    }
}
