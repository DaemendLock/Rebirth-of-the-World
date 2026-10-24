using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.UseCases
{
    public readonly struct HealthSetUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IHealthOutput _healthOutput;

        public HealthSetUseCase(IUnitStore unitStore, IHealthOutput healthOutput)
        {
            _unitStore = unitStore;
            _healthOutput = healthOutput;
        }

        public void Execute(UnitId target, float value)
        {
            ref HealthOwner healthOwner = ref _unitStore.Get(target).Health;
            float oldValue = healthOwner.Current;
            healthOwner.Current = value;
            _healthOutput.Present(new HealthChangeRecord(healthOwner.Id, healthOwner.Current, healthOwner.Max, oldValue));
        }
    }

    public readonly struct ComposureSetUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IHealthOutput _healthOutput;

        public ComposureSetUseCase(IUnitStore unitStore, IHealthOutput healthOutput)
        {
            _unitStore = unitStore;
            _healthOutput = healthOutput;
        }

        public void Execute(UnitId target, float value)
        {
            UnitContext unit = _unitStore.Get(target);
            float oldValue = unit.Composure.Current;
            unit.Composure.Current = value;
            _healthOutput.Present(new ComposureChangeRecord(unit.Composure.Id, unit.Composure.Current, unit.Composure.Max, oldValue));
        }
    }

    public interface IHealthOutput
    {
        void Present(HealthChangeRecord record);
        void Present(ComposureChangeRecord record);
        void Present(DamageInstance instance);
    }
}
