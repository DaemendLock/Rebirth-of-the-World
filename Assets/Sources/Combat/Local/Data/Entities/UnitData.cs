using Combat.Local.Domain.OldAttributes;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Data.Entities
{
    public class UnitData : ScriptableObject
    {
        [field: SerializeField] public object? Prefab { get; private set; }
    }

    public interface IUnitData
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        IAttributeCollection<Attribute> Stats { get; }
        object Resources { get; }
        CastableSkillData[] SkillDatas { get; }
        GameObject Prefab { get; }
    }
}
