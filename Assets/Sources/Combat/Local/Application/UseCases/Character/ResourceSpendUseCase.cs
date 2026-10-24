using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public readonly struct ResourceSpendUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ISpendResourceOutput _spendResourceOutput;

        public ResourceSpendUseCase(IUnitStore units, ISpendResourceOutput spendResourceOutput)
        {
            _unitStore = units;
            _spendResourceOutput = spendResourceOutput;
        }

        public void Execute(UnitId target, ResourceId resource, float value, AbilityKey? abilityId)
        {
            ResourceOwner resourceOwner = _unitStore.Get(target).Resources;
            resourceOwner.TrySpendResource(resource, value);

            _spendResourceOutput.Present(new(target, resource, resourceOwner.GetResource(resource).MaxValue, resourceOwner.GetResource(resource).Value));
        }
    }
}
