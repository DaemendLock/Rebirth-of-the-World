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

        public GiveResourceUseCase(IResourceRepository resourceRepository, IGiveResourceOutput giveResourceOutput)
        {
            _resourceRepository = resourceRepository;
            _giveResourceOutput = giveResourceOutput;
        }

        public void Execute(EntityId target, ResourceId resourceType, float value, EventSource source)
        {
            Resource resource = _resourceRepository.Get(target, resourceType);
            resource.Fill(value);
            _resourceRepository.Update(resource);

            GiveResourceResult result = new(target, resourceType, value, resource.CurrentValue, resource.MaxValue, source.Skill, source.Unit);
            _giveResourceOutput.Present(result);
        }
    }
}
