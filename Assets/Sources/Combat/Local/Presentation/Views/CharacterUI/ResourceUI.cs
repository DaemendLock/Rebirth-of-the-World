using Combat.View.Common;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.View.CharacterUI
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private List<Bar> _bars;
        [SerializeField] private Bar _barPrefab;

        public void CreateBar(Vector2 localPosition, FillDirection barDirection)
        {
            Bar bar = Instantiate(_barPrefab, transform);
            _bars.Add(bar);
            bar.FillDirection = barDirection;
            bar.transform.localPosition = localPosition;
        }
    }
}
