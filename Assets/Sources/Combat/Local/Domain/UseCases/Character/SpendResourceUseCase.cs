using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct SpendResourceUseCase
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ISpendResourceOutput _spendResourceOutput;

        public SpendResourceUseCase(IResourceRepository resourceRepository, ISpendResourceOutput spendResourceOutput)
        {
            _resourceRepository = resourceRepository;
            _spendResourceOutput = spendResourceOutput;
        }

        public void Execute(EntityId target, ResourceId resource, float value, EventSource source)
        {
            Resource resourceValue = _resourceRepository.Get(target, resource);
            resourceValue.Spend(value);

            _resourceRepository.Update(resourceValue);
            _spendResourceOutput.Present(resourceValue);
        }
    }
}
