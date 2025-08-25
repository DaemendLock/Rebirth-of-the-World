using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.Services
{
    public class ResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public float GetResourceValue(EntityId id, ResourceId resource) => _resourceRepository.Get(id, resource).CurrentValue;

        public Resource GiveResource(EntityId owner, ResourceId resource, float value)
        {
            Resource data = _resourceRepository.Get(owner, resource);
            data.CurrentValue += value;

            if (data.CurrentValue > data.MaxValue)
            {
                data.CurrentValue = data.MaxValue;
            }

            _resourceRepository.Upgrade(data);
            return data;
        }

        public Resource SpendResource(EntityId owner, ResourceId resource, float value)
        {
            Resource data = _resourceRepository.Get(owner, resource);
            data.CurrentValue -= value;

            if (data.CurrentValue < 0)
            {
                data.CurrentValue = 0;
            }

            _resourceRepository.Upgrade(data);
            return data;
        }
    }
}
