using Server.Combat.Domain.Attributes;

using UnityEngine;

namespace Server.Combat.Domain.Entities
{
    public class Transform
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
    }

    public class Killable
    {
        public float DefaultHealth { get; }
        public float CurrentHealth { get; set; }
        public bool Alive { get; set; }
    }

    public class AttributesOwner
    {
        public float GetMaxHealthBonus() => GetAttributeValue(Attribute.Endurance);

        public IAttributeCollection<Attribute> GetBaseAttributes() => throw new System.NotImplementedException();

        public float GetAttributeValue(Attribute attribute) => throw new System.NotImplementedException();
    }
}
