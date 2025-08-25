using System.Collections.Generic;
using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Server.Data.Repositories.Unit
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly Dictionary<(EntityId, ResourceId), Resource> _values = new();

        public void Create(Resource resource) => _values.Add((resource.Owner, resource.ResourceId), resource);
        public void Delete(EntityId entityId, ResourceId resourceId) => _values.Remove((entityId, resourceId));
        public Resource Get(EntityId entityId, ResourceId resourceId) => _values.GetValueOrDefault((entityId, resourceId), new(entityId, resourceId, 0, 0));
        public void Upgrade(Resource resource) => _values[(resource.Owner, resource.ResourceId)] = resource;
    }

    public class CastActionRepository : IActionRepository
    {
        private readonly Dictionary<EntityId, IAction> _values = new();

        public void Create(IAction value) => _values.Add(value.Actor, value);

        public void Delete(EntityId id) => _values.Remove(id);

        public IAction Get(EntityId id) => _values.GetValueOrDefault(id, null);

        public void Update(IAction value) => _values[value.Actor] = value;

        public IEnumerable<IAction> GetAll() => _values.Values;
    }
}
