using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IAttributesRepository
    {
        void Create(AttributesOwner attributes);
        void Update(AttributesOwner attributes);
        AttributesOwner Get(UnitId entityId);
        void Delete(UnitId id);
    }

    public interface ITransformEffectRepository
    {

    }
}
