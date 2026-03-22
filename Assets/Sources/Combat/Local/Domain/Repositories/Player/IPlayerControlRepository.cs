using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Repositories.Player
{
    public interface IPlayerControlRepository
    {
        void Create(PlayerId player, EntityId id);
        EntityId Get(PlayerId id);
        void Update(PlayerId player, EntityId id);
        void Delete(PlayerId player);
    }
}
