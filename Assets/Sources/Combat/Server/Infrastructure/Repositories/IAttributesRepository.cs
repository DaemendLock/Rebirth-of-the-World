using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Units.ValueObjects;

namespace Server.Combat.Infrastructure.Repositories
{
    public interface IAttributesRepository
    {
        IAttributeCollection<Attribute> Create(EntityId id, IAttributeCollection<Attribute> defaultValue);
        void Remove(EntityId id);

        IAttributeCollection<Attribute> Get(EntityId id);
        void Reset();
    }
}
