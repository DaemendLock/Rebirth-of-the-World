using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

using System;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusOwnerRepository
    {
        void Create(StatusOwner statusOwner);
        void Delete(UnitId id);
        ref StatusOwner Get(UnitId id);
        Span<StatusOwner> GetAll();
    }
}
