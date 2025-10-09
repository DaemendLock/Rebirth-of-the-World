using Combat.API.DTO;

namespace Combat.API.Statuses
{
    public interface IResourceGainSpendHandler : IStatusPropery
    {
        void OnGainResource(ResourceChangeRecord @event);
        void OnSpendResource(ResourceChangeRecord @event);
    }
}
