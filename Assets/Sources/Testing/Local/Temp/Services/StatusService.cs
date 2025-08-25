using Combat.Common.ValueObjects;
using Combat.Local.Domain.API;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Infrastructure.Factories;

using Testing.Local.Temp.Factories;

namespace Testing.Local.Temp.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly StatusApiRepository _statusApiRepository;
        private readonly IStatusFactory _statusFactory;
        private readonly StatusApiFactory _statusApiFactory;

        public StatusService(IStatusRepository statusRepository, StatusApiRepository statusApiRepository, IStatusFactory statusFactory, StatusApiFactory statusApiFactory)
        {
            _statusRepository = statusRepository;
            _statusFactory = statusFactory;
            _statusRepository = statusRepository;
            _statusApiRepository = statusApiRepository;
            _statusFactory = statusFactory;
            _statusApiFactory = statusApiFactory;
        }

        public void ApplyStatus(StatusName statusName, EntityId parent, EntityId caster, SkillId source, float duration, int stackCount)
        {
            Status status = _statusFactory.Create(statusName, parent, caster, source, duration, stackCount);

            if (status.Parent != parent)
            {
                return;
            }

            _statusRepository.Create(status);

            //_api.CreateApi(status);
            StatusApi api = _statusApiFactory.Create(status.Id);
            _statusApiRepository.Create(api);
            api.OnCreate();

            return;
        }

        public void RemoveStatus(StatusId effect)
        {
            _statusRepository.Delete(effect);
        }
    }
}
