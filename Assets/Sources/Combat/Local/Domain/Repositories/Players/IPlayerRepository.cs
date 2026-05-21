using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface IPlayerRepository
    {
        void Create(Player player);
        Player Get(PlayerId id);
        void Update(Player player);
        void Delete(PlayerId player);

        bool TryFindOnwer(UnitId entityId, out Player player);
    }
}
