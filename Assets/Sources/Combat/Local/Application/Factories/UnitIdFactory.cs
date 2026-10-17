using Combat.Common.Primitives;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.Factories
{
    public class UnitIdFactory
    {
        private readonly ICharacterUpdateRepository _characterUpdateRepository;
        private int _nextId = 0;

        public UnitIdFactory(ICharacterUpdateRepository characterUpdateRepository)
        {
            _characterUpdateRepository = characterUpdateRepository;
        }

        public UnitId GetId()
        {
            while (_characterUpdateRepository.TryGet(new(_nextId), out _))
            {
                _nextId++;
            }

            return new(_nextId++);
        }
    }
}
