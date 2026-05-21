using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusRepository
    {
        void Create(Status effect);
        void Update(Status effect);
        void Delete(StatusId effect);
        bool TryGet(StatusId id, out Status effect);
    }
}
