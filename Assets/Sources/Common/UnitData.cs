using System;

using Server.Combat.Data.Entities;
using Server.Combat.Domain.Attributes;

using UnityEngine;

namespace Assets.Sources.Common
{
    public class UnitData// : IUnitData
    {
        public float MaxHealth => throw new NotImplementedException();

        public float CurrentHealth => throw new NotImplementedException();

        public IAttributeCollection<Server.Combat.Domain.Attributes.Attribute> Stats => throw new NotImplementedException();

        public object Resources => throw new NotImplementedException();

        public SkillData[] SkillDatas => throw new NotImplementedException();

        public GameObject Prefab => throw new NotImplementedException();
    }
}
