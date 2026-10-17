using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IAttributesRepository
    {
        void Create(AttributesOwner attributes);
        AttributesOwner Get(UnitId entityId);
        void Delete(UnitId id);
    }
}
