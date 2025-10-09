using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct GiveResourceUseCase
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IGiveResourceOutput _giveResourceOutput;
        private readonly IGiveResourceEventHandler _eventHandler;

        public GiveResourceUseCase(IResourceRepository resourceRepository, IGiveResourceEventHandler eventHandler, IGiveResourceOutput giveResourceOutput)
        {
            _resourceRepository = resourceRepository;
            _eventHandler = eventHandler;
            _giveResourceOutput = giveResourceOutput;
        }

        public void Execute(EntityId target, ResourceId resource, float value, EventSource source)
        {
            Resource result = _resourceRepository.Get(target, resource);
            result.CurrentValue += value;

            if (result.CurrentValue > result.MaxValue)
            {
                result.CurrentValue = result.MaxValue;
            }

            _resourceRepository.Update(result);
            _giveResourceOutput.Present(result);
            _eventHandler.HandleEvent(target, resource, value, source);
        }
    }

    public interface IGiveResourceOutput
    {
        void Present(Resource resource);
    }

    public interface IGiveResourceEventHandler
    {
        void HandleEvent(EntityId unit, ResourceId resource, float value, EventSource source);
    }
}
