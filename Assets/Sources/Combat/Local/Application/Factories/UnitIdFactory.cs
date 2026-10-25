using Combat.Common.Primitives;
using Combat.Domain.Repositories;

namespace Combat.Local.Application.Factories
{
    public class UnitIdFactory
    {
        private readonly IUnitStore _unitStore;
        private int _nextId = 0;

        public UnitIdFactory(IUnitStore unitStore)
        {
            _unitStore = unitStore;
            _nextId = 0;
        }

        public UnitId GetId()
        {
            while (_unitStore.TryGet(new(_nextId), out _))
            {
                _nextId++;
            }

            return new(_nextId++);
        }
    }
}
