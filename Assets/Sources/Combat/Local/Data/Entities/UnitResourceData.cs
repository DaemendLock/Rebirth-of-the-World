using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Data.Entities
{
    public class UnitResourceData : ScriptableObject
    {
        [SerializeField] private int _id;

        public UnitResourceData(ResourceId id, string name, Color color)
        {
            _id = id.Value;
            Name = name;
            Color = color;
        }

        public ResourceId Id => new(_id);
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}
