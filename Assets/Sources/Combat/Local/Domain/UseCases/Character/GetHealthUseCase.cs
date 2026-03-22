using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct GetHealthUseCase
    {
        private readonly IHealthRepository _healthRepository;

        public GetHealthUseCase(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        public Health Execute(EntityId target)
        {
            return _healthRepository.Get(target);
        }
    }
}
