using Combat.Local.Domain.Entities.Units;

using System;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    [RequireComponent(typeof(CharacterModelComponent))]
    public sealed class ScaleEffectComponent : MonoBehaviour
    {
        private CharacterModelComponent _characterModel;

        public ScaleOverTimeEffect[] Values { get; set; } = Array.Empty<ScaleOverTimeEffect>();

        private void Start()
        {
            _characterModel = GetComponent<CharacterModelComponent>();
        }

        private void Update()
        {
            float currentScale = transform.localScale.x;
            float rate = 0;

            foreach (var value in Values)
            {
                rate += value.Rate;
            }

            currentScale += rate * Time.deltaTime * _characterModel.TimeScale;
            transform.localScale = Vector3.one * currentScale;
        }
    }
}
