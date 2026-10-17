using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct StatusOwnerFindStatusUseCase
    {
        private readonly IStatusOwnerRepository _statusRepository;

        public StatusOwnerFindStatusUseCase(IStatusOwnerRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public StatusId? FindStatus(UnitId id, StatusType statusName)
        {
            if (_statusRepository.TryGet(id, out StatusOwner target) == false)
            {
                return null;
            }

            foreach (StatusInstance item in target.GetAll())
            {
                if (item.Name != statusName)
                {
                    continue;
                }

                return item.Id;
            }

            return default;
        }
    }
}
