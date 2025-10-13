using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class ResourceRepository : IResourceRepository
    {
        private readonly Dictionary<(EntityId, ResourceId), Resource> _values = new();

        public void Create(Resource resource) => _values.Add((resource.Id, resource.ResourceId), resource);
        public void Delete(EntityId entityId, ResourceId resourceId) => _values.Remove((entityId, resourceId));
        public Resource Get(EntityId entityId, ResourceId resourceId) => _values.GetValueOrDefault((entityId, resourceId), new(entityId, resourceId, 0, 0));
        public void Update(Resource resource) => _values[(resource.Id, resource.ResourceId)] = resource;
    }
}
