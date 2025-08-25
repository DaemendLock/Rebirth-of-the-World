using Server.Combat.Domain.Attributes;

namespace Server.Combat.Domain.Implementations.Items
{
    public interface IEquipable
    {
        void Equip();
    }

    public interface IItem
    {
        public void GetAttributeBonus(IAttributeCollection<Attribute> attributeCollection);
    }
}
