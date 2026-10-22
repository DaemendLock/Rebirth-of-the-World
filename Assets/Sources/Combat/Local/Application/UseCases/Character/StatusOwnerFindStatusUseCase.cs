using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public readonly struct StatusOwnerFindStatusUseCase
    {
        private readonly IUnitStore _unitStore;

        public StatusOwnerFindStatusUseCase(IUnitStore units)
        {
            _unitStore = units;
        }

        public StatusId? FindStatus(UnitId id, StatusType statusName)
        {
            try
            {
                foreach (StatusInstance item in _unitStore.Get(id).Statuses.GetAll())
                {
                    if (item.Name != statusName)
                    {
                        continue;
                    }

                    return item.Id;
                }

                return default;
            }
            catch
            {
                return null;
            }
        }
    }
}
