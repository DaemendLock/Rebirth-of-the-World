using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
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
            Resource resourceValue = _resourceRepository.Get(target, resource);
            resourceValue.CurrentValue += value;

            if (resourceValue.CurrentValue > resourceValue.MaxValue)
            {
                resourceValue.CurrentValue = resourceValue.MaxValue;
            }

            _resourceRepository.Update(resourceValue);
            GiveResourceResult result = new(target, resource, value, resourceValue.CurrentValue, resourceValue.MaxValue, source.Skill, source.Unit);
            _giveResourceOutput.Present(result);
            _eventHandler.HandleEvent(result);
        }
    }
}
