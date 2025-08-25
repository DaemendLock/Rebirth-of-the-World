using System;
using System.Collections.Generic;
using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;

using Testing.Local.Temp.Factories;

namespace Testing.Local.Temp.Services
{
    public class StatusLookupService : IStatusLookupService
    {
        private readonly StatusApiRepository _apiRepository;

        private ILookup<EntityId, StatusApi> _statusEffects;

        public StatusLookupService(StatusApiRepository statusRepository)
        {
            _apiRepository = statusRepository;
            Update();
        }

        public void Update() => _statusEffects = _apiRepository.GetAll().ToLookup(value => value.Parent.Id);

        public IEnumerable<StatusApi> FindStatusesOnUnit(EntityId owner) => _statusEffects.Contains(owner) ? _statusEffects[owner] : Array.Empty<StatusApi>();
    }
}
