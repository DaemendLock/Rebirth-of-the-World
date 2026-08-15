using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
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
            Span<ScaleOverTimeEffect> values = stackalloc ScaleOverTimeEffect[owner.Values.Length + 1];
            owner.Values.CopyTo(values);
            values[^1] = effect;
            _repository.Update(new(owner.Id, values));
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

            Span<ScaleOverTimeEffect> values = stackalloc ScaleOverTimeEffect[owner.Values.Length];
            int cursor = 0;

            for (int i = 0; i < owner.Values.Length; i++)
            {
                if (owner.Values[i].Id != effectId)
                {
                    values[cursor++] = owner.Values[i];
                }
            }

            _repository.Update(new(owner.Id, values[..cursor]));

        }
    }
}
