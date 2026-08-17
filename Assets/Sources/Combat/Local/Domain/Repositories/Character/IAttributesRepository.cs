using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

using System;

namespace Combat.Local.Domain.Repositories
{
    public interface IAttributesRepository
    {
        void Create(AttributesOwner attributes);
        ref AttributesOwner Get(UnitId entityId);
        Span<AttributesOwner> GetAll();
        void Delete(UnitId id);
    }

    public interface ITransformEffectRepository
    {

    }
}
