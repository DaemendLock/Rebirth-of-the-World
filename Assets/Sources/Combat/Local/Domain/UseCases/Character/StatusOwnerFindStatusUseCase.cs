using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

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

            foreach (StatusId item in statusOwner.GetAll())
            {
                if (_statusRepository.TryGet(item, out Status status) == false)
                {
                    continue;
                }

                if (status.Name != statusName)
                {
                    continue;
                }

                return item;
            }

            return default;
        }
    }
}
