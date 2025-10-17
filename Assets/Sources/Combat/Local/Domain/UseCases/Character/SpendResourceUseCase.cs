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
        private readonly ISpendResourceEventHandler _spendResourceEventHandler;

        public SpendResourceUseCase(IResourceRepository resourceRepository, ISpendResourceOutput spendResourceOutput, ISpendResourceEventHandler spendResourceEventHandler)
        {
            _resourceRepository = resourceRepository;
            _spendResourceOutput = spendResourceOutput;
            _spendResourceEventHandler = spendResourceEventHandler;
        }

        public void Execute(EntityId target, ResourceId resource, float value, EventSource source)
        {
            Resource resourceValue = _resourceRepository.Get(target, resource);

            resourceValue.CurrentValue -= value;

            if (resourceValue.CurrentValue < 0)
            {
                resourceValue.CurrentValue = 0;
            }

            _resourceRepository.Update(resourceValue);
            _spendResourceOutput.Present(resourceValue);
            SpendResourceResult result = new(target, resource, value, source.Skill, source.Unit);
            _spendResourceEventHandler.HandleEvent(result);
        }
    }
}
