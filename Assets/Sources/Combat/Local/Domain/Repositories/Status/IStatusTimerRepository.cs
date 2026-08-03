using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface IStatusTimerRepository
    {
        void Create(StatusTimer statusTimer);
        void Update(StatusTimer statusTimer);
        void Delete(StatusId statusId);
        bool TryGet(StatusId statusId, out StatusTimer statusTimer);

        ICollection<StatusTimer> GetAll();
    }
}
