using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

using System.Linq;

namespace Combat.Local.Domain.UseCases
{
    public class PrecacheAttributersUseCase
    {
        private readonly IAttributesRepository _attributesRepository;

        public PrecacheAttributersUseCase(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;
        }

        public void Execute(float deltaTime)
        {
            _attributesRepository.ClearCache();

            foreach (EntityId value in _attributesRepository.GetAllIds().ToArray())
            {
                _attributesRepository.Get(value);
            }
        }
    }
}
