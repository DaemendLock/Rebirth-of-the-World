using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusRepository
    {
        void Create(Status effect);
        void Update(Status effect);
        void Delete(StatusId effect);
        bool TryGet(StatusId id, out Status effect);
        ICollection<Status> GetAll();
    }
}
