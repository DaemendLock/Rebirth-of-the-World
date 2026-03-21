using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class StatusOwnerRepository : IStatusOwnerRepository
    {
        private readonly Dictionary<EntityId, StatusId[]> _statusRepository;

        public StatusOwnerRepository()
        {
            _statusRepository = new();
        }

        public void Create(StatusOwner statusOwner) => throw new System.NotImplementedException();
        public void Delete(StatusOwner statusOwner) => throw new System.NotImplementedException();

        public StatusOwner Get(EntityId id)
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
