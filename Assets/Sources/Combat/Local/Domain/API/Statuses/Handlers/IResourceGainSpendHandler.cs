using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IResourceGainSpendHandler : IStatusPropery
    {
        void OnGainResource(ResourceChangeRecord @event);
        void OnSpendResource(ResourceChangeRecord @event);
    }
}
