using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
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
            Resource result = _resourceRepository.Get(target, resource);

            result.CurrentValue -= value;

            if (result.CurrentValue < 0)
            {
                result.CurrentValue = 0;
            }

            _resourceRepository.Update(result);
            _spendResourceEventHandler.HandleEvent(target, resource, value, source);
            _spendResourceOutput.Present(result);
        }
    }

    public interface ISpendResourceOutput
    {
        void Present(Resource resource);
    }

    public interface ISpendResourceEventHandler
    {
        void HandleEvent(EntityId target, ResourceId resource, float value, EventSource source);
    }
}
