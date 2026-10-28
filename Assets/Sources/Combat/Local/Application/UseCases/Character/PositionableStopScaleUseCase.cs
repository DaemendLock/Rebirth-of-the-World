using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.Factories;

namespace Combat.Local.Application.UseCases
{
    public interface ICharacterScaleOverTimeOutput
    {
        void Present(UnitId target, float scaleRate);
    }

    public sealed class PositionableStartScaleUseCase
    {
        private readonly ScaleEffectFactory _factory;
        private readonly IUnitStore _unitStore;
        private readonly ICharacterScaleOverTimeOutput _output;

        public PositionableStartScaleUseCase(ScaleEffectFactory factory, IUnitStore unitStore, ICharacterScaleOverTimeOutput output)
        {
            _factory = factory;
            _unitStore = unitStore;
            _output = output;
        }

        public ScaleEffectId Execute(UnitId target, float rate)
        {
            var unit = _unitStore.Get(target);
            ScaleOverTimeEffect effect = _factory.Create(target, rate);
            unit.Positionable.StartScaleOverTime(effect);
            _output.Present(target, unit.Positionable.GetScaleRate());
            return effect.Id;
        }
    }

    public sealed class PositionableStopScaleUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ICharacterScaleOverTimeOutput _output;

        public PositionableStopScaleUseCase(IUnitStore unitStore, ICharacterScaleOverTimeOutput output)
        {
            _unitStore = unitStore;
            _output = output;
        }

        public void Execute(ScaleEffectId effectId)
        {
            var unit = _unitStore.Get(effectId.OwnerId);
            unit.Positionable.StopScaleOverTime(effectId);
            _output.Present(unit.Id, unit.Positionable.GetScaleRate());
        }
    }
}
