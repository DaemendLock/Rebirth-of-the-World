using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public readonly struct ResourceGiveUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IGiveResourceOutput _giveResourceOutput;

        public ResourceGiveUseCase(IUnitStore units, IGiveResourceOutput giveResourceOutput)
        {
            _unitStore = units;
            _giveResourceOutput = giveResourceOutput;
        }

        public void Execute(UnitId target, ResourceId resourceType, float value, AbilityKey? abilityId)
        {
            ResourceOwner resourceOwner = _unitStore.Get(target).Resources;
            resourceOwner.FillResource(resourceType, value);

            GiveResourceResult result = new(target, resourceType, value, resourceOwner.GetResource(resourceType).Value, resourceOwner.GetResource(resourceType).MaxValue, abilityId);
            _giveResourceOutput.Present(result);
        }
    }
}
