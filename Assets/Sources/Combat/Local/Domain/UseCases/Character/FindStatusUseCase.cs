using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct FindStatusUseCase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusId? FindStatus(EntityId id, StatusName statusName)
        {
            foreach (Status item in _statusRepository.GetAll())
            {
                if (item.Parent == id && item.Name == statusName)
                {
                    return item.Id;
                }
            }

            return default;
        }
    }
}
