using UnityEngine;

namespace Combat.Local.Presentation.Views
{
    [RequireComponent(typeof(CharacterView))]
    public sealed class ScalableViewComponent : MonoBehaviour
    {
        private CharacterView _characterModel;

        private void Start()
        {
            _characterModel = GetComponent<CharacterView>();
        }
        public float ScaleRate { get; set; }

        private void Update()
        {
            float currentScale = transform.localScale.x;

            currentScale += ScaleRate * Time.deltaTime * _characterModel.TimeScale;
            transform.localScale = Vector3.one * currentScale;
        }
    }
}
