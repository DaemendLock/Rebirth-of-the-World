using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Factories;
using Combat.Local.Application.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class PositionableStartScaleUseCase
    {
        private readonly ScaleEffectFactory _factory;
        private readonly IScaleEffectOwnerRepository _repository;

        public PositionableStartScaleUseCase(ScaleEffectFactory factory, IScaleEffectOwnerRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public ScaleEffectId Execute(UnitId target, float rate)
        {
            ScaleEffectOwner owner = _repository.Get(target);
            ScaleOverTimeEffect effect = _factory.Create(owner.Id, rate);
            owner.Add(effect);
            return effect.Id;
        }
    }

    public sealed class PositionableStopScaleUseCase
    {
        private readonly IScaleEffectOwnerRepository _repository;

        public PositionableStopScaleUseCase(IScaleEffectOwnerRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ScaleEffectId effectId)
        {
            ScaleEffectOwner owner = _repository.Get(effectId.OwnerId);
            owner.Remove(effectId);
        }
    }
}
