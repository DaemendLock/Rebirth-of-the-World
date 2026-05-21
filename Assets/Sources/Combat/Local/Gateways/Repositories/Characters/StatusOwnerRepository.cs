using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class StatusOwnerRepository : IStatusOwnerRepository
    {
        private readonly Dictionary<UnitId, StatusId[]> _statusRepository;

        public StatusOwnerRepository()
        {
            _statusRepository = new();
        }

        public void Create(StatusOwner statusOwner) => _statusRepository.Add(statusOwner.Id, statusOwner.GetAll().ToArray());

        public void Delete(UnitId id) => _statusRepository.Remove(id);

        public StatusOwner Get(UnitId id)
        {
            if (_statusRepository.TryGetValue(id, out StatusId[] values) == false)
            {
                return new(id, Array.Empty<StatusId>());
            }

            return new(id, values);
        }

        public void Update(StatusOwner statusOwner) => _statusRepository[statusOwner.Id] = statusOwner.GetAll().ToArray();
    }
}
