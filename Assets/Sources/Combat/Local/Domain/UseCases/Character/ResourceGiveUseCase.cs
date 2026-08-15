using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ResourceGiveUseCase
    {
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly IGiveResourceOutput _giveResourceOutput;

        public ResourceGiveUseCase(IResourceOwnerRepository resourceRepository, IGiveResourceOutput giveResourceOutput)
        {
            _resourceRepository = resourceRepository;
            _giveResourceOutput = giveResourceOutput;
        }

        public void Execute(UnitId target, ResourceId resourceType, float value, AbilityKey? abilityId)
        {
            ResourceOwner resourceOwner = _resourceRepository.Get(target);
            resourceOwner.FillResource(resourceType, value);
            _resourceRepository.Update(resourceOwner);

            GiveResourceResult result = new(target, resourceType, value, resourceOwner.GetResource(resourceType).Value, resourceOwner.GetResource(resourceType).MaxValue, abilityId);
            _giveResourceOutput.Present(result);
        }
    }
}
