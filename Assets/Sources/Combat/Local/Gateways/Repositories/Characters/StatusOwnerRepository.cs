using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class StatusOwnerRepository : IStatusOwnerRepository
    {
        private readonly RefContainer<UnitId, StatusOwner> _values;

        public StatusOwnerRepository()
        {
            _values = new();
        }

        public void Create(StatusOwner statusOwner) => _values.Add(statusOwner.Id, statusOwner);

        public void Delete(UnitId id) => _values.Remove(id);

        public ref StatusOwner Get(UnitId id) => ref _values.Get(id);
        public bool TryGet(UnitId id, out StatusOwner statusOwner)
        {
            try
            {
                statusOwner = _values.Get(id);
                return true;
            }
            catch
            {
                statusOwner = default;
                return false;
            }
        }
    }
}
