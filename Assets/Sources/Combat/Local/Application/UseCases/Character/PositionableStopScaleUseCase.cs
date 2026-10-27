using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Factories;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases
{
    public interface ICharacterScaleOverTimeOutput
    {
        void Present(UnitId target, float scaleRate);
    }

    public sealed class PositionableStartScaleUseCase
    {
        private readonly ScaleEffectFactory _factory;
        private readonly IPositionableRepository _repository;
        private readonly ICharacterScaleOverTimeOutput _output;

        public PositionableStartScaleUseCase(ScaleEffectFactory factory, IPositionableRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public ScaleEffectId Execute(UnitId target, float rate)
        {
            Positionable positionable = _repository.Get(target);
            ScaleOverTimeEffect effect = _factory.Create(target, rate);
            positionable.StartScaleOverTime(effect);
            _output.Present(target, positionable.GetScaleRate());
            return effect.Id;
        }
    }

    public sealed class PositionableStopScaleUseCase
    {
        private readonly IPositionableRepository _repository;
        private readonly ICharacterScaleOverTimeOutput _output;

        public PositionableStopScaleUseCase(IPositionableRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ScaleEffectId effectId)
        {
            Positionable positionable = _repository.Get(effectId.OwnerId);
            positionable.StopScaleOverTime(effectId);
            _output.Present(positionable.Id, positionable.GetScaleRate());
        }
    }
}
