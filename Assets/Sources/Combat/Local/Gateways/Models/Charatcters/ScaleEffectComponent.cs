using Combat.Domain.Entities;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    [RequireComponent(typeof(CharacterModelComponent))]
    public sealed class ScaleEffectComponent : MonoBehaviour
    {
        private CharacterModelComponent _characterModel;

        private void Start()
        {
            _characterModel = GetComponent<CharacterModelComponent>();
        }
        public ScaleEffectOwner? Data { get; set; }

        private void Update()
        {
            if (Data.HasValue == false)
            {
                return;
            }

            float currentScale = transform.localScale.x;
            float rate = 0;

            foreach (var value in Data.Value.Values)
            {
                rate += value.Rate;
            }

            currentScale += rate * Time.deltaTime * _characterModel.TimeScale;
            transform.localScale = Vector3.one * currentScale;
        }
    }
}
