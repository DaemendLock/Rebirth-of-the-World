using Server.Combat.Domain.Attributes;

namespace Server.Combat.Data.Entities
{
    public interface IUnitData
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        IAttributeCollection<Attribute> Stats { get; }
        object? Resources { get; }
        SkillData[] SkillDatas { get; }
        UnityEngine.GameObject Prefab { get; }
    }
}
