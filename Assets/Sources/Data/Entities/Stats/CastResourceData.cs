using System;
using UnityEngine;
using Utils.DataTypes;

namespace Data.Entities.Stats
{
    [Serializable]
    public class CastResourceData
    {
        [SerializeField] private TypedResource _leftResource;
        [SerializeField] private TypedResource _rightResource;

        public TypedResource Left => _leftResource;
        public TypedResource Right => _rightResource;
    }

    [Serializable]
    public class TypedResource
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private float _maxValue;

        public ResourceType Type => _type;
        public Resource Resource => new(_maxValue);
    }
}
