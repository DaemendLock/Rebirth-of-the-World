using Combat.Local.Domain.API;
using Testing.Local.Temp.Factories;
using System.Collections.Generic;
using Combat.Local.Domain.API.Statuses;

namespace Testing.Local.Temp.Services
{
    public class StatusApiUpdateService
    {
        private readonly StatusApiRepository _statusApiRepository;
        private readonly IStatusLookupService _statusLookupService;

        private readonly List<StatusApi> _buffer;

        public StatusApiUpdateService(StatusApiRepository statusApiRepository, IStatusLookupService statusLookupService)
        {
            _statusApiRepository = statusApiRepository;
            _statusLookupService = statusLookupService;

            _buffer = new();
        }

        public void Update()
        {
            ICollection<StatusApi> values = _statusApiRepository.GetAll();

            lock (_buffer)
            {

                if (_buffer.Capacity < values.Count)
                {
                    _buffer.Capacity = values.Count;
                }

                _buffer.Clear();
                _buffer.AddRange(values);

                foreach (StatusApi value in _buffer)
                {
                    if (value.Update() == false)
                    {
                        value.OnRemove();
                        _statusApiRepository.Delete(value.Id);
                        continue;
                    }
                }
            }

            _statusLookupService.Update();
        }
    }
}
