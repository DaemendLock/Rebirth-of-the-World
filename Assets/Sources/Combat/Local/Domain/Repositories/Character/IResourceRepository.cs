using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IResourceRepository
    {
        void Create(Resource resource);
        void Update(Resource resource);
        Resource Get(EntityId entityId, ResourceId resourceId); 
        void Delete(EntityId entityId, ResourceId resourceId);
    }
}
