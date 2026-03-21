using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IAttributesRepository
    {
        void Create(AttributesOwner attributes);
        void Update(AttributesOwner attributes);
        AttributesOwner Get(EntityId entityId);
        void Delete(EntityId id);
    }

    public interface ITransformEffectRepository
    {
        
    }
}
