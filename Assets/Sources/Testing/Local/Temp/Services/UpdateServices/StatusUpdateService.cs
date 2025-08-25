using System.Collections.Generic;

using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Domain.ValueObjects;

namespace Testing.Local.Temp.Services
{
    public class StatusUpdateService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusService _statusService;

        private readonly List<Status> _buffer;

        public StatusUpdateService(IStatusRepository statusRepository, IStatusService statusService)
        {
            _statusRepository = statusRepository;
            _statusService = statusService;

            _buffer = new();
        }

        public void Update(float deltaTime)
        {
            ICollection<Status> values = _statusRepository.GetAll();

            if (_buffer.Capacity < values.Count)
            {
                _buffer.Capacity = values.Count;
            }

            _buffer.AddRange(values);

            foreach (Status value in _buffer)
            {
                Duration duration = value.Duration;

                if (value.Duration.Left <= 0)
                {
                    _statusService.RemoveStatus(value.Id);
                    continue;
                }

                duration.ActiveTime += deltaTime;
                Status status = value;
                status.Duration = duration;
                _statusRepository.Update(status);
            }

            _buffer.Clear();
        }
    }
}
