using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Repositories
{
    public interface IAttributesRepository
    {
        void Create(AttributesOwner attributes);
        void Update(AttributesOwner attributes);
        AttributesOwner Get(UnitId entityId);
        void Delete(UnitId id);
    }
}
