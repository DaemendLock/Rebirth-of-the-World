using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Components
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private string _unitName;

        [field: SerializeField, Min(0)] public float Mass { get; private set; }

        public ModelName UnitName
        {
            get => new(_unitName);
            set => _unitName = value.Value;
        }
    }
}
