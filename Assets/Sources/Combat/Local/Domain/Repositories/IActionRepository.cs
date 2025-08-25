using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IActionRepository
    {
        IAction Get(EntityId id);
        void Create(IAction id);
        void Update(IAction value);
        void Delete(EntityId id);

        IEnumerable<IAction> GetAll();
    }
}
