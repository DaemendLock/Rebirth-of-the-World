using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Domain.Repositories
{
    public interface IStatusTimerRepository
    {
        void Create(StatusTimer statusTimer);
        void Update(StatusTimer statusTimer);
        void Delete(StatusId statusId);
        bool TryGet(StatusId statusId, out StatusTimer statusTimer);
    }
}
