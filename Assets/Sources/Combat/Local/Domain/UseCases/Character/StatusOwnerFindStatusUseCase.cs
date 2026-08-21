using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct StatusOwnerFindStatusUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;

        public StatusOwnerFindStatusUseCase(IStatusRepository statusRepository, IStatusOwnerRepository statusOwnerRepository)
        {
            _statusRepository = statusRepository;
            _statusOwnerRepository = statusOwnerRepository;
        }

        public StatusId? FindStatus(UnitId id, StatusType statusName)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(id);

            foreach (StatusInstance item in statusOwner.GetAll())
            {
                if (item.Type != statusName)
                {
                    continue;
                }

                return item.StatusId;
            }

            return default;
        }
    }
}
