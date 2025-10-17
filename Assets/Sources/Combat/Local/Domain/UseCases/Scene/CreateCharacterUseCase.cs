using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using System;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class CreateCharacterUseCase
    {
        private readonly UnitIdFactory _unitModelFactory;

        private readonly IHealthRepository _healthRepository;
        private readonly IKillableRepository _killableRepository;
        private readonly IAligmentRepository _aligmentRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;

        private readonly ICreateUnitEventHandler _createUnitEventHandler;

        private readonly ICreateUnitOutput _outputPort;

        public CreateCharacterUseCase(
            IHealthRepository healthRepository, IKillableRepository killableRepository, IAligmentRepository aligmentRepository,
            IAttributesRepository attributesRepository, IResourceRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
            IPositionableRepository positionableRepository, IActorRepository actorRepository,
            ICreateUnitEventHandler createUnitEventHandler,
            ICreateUnitOutput outputPort)
        {
            _healthRepository = healthRepository;
            _killableRepository = killableRepository;
            _aligmentRepository = aligmentRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _createUnitEventHandler = createUnitEventHandler;
            _outputPort = outputPort;

            _unitModelFactory = new();
        }

        public void Execute(CreateCharacterDTO data)
        {
            Positionable positionable = Create(data);

            _outputPort.Present(positionable);
            _positionableRepository.Update(positionable);
            SkillOwner skillOwner = _skillOwnerRepository.Get(positionable.Id);
            _createUnitEventHandler.HandleEvent(positionable.Id, skillOwner.GetAll());
        }

        public void Execute(CreateCharacterDTO data, Transform parent)
        {
            Positionable positionable = Create(data);

            _outputPort.SetTransform(positionable, parent);
            _positionableRepository.Update(positionable);
            SkillOwner skillOwner = _skillOwnerRepository.Get(positionable.Id);
            _createUnitEventHandler.HandleEvent(positionable.Id, skillOwner.GetAll());
        }

        private Positionable Create(CreateCharacterDTO context)
        {
            EntityId id = _unitModelFactory.GetId();

            Killable killable = new(id, true);

            Health health = new(id, context.DefaultHealth)
            {
                CurrentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth,
            };

            Aligment aligment = new(id, context.Team);

            AttributeValue[] attributeValue = new AttributeValue[Attributes.AttributeCount];

            for (int i = 0; i < Attributes.AttributeCount; i++)
            {
                if (i < context.DefaultAttributes.Length)
                {
                    attributeValue[i] = context.DefaultAttributes[i];
                }
            }

            Attributes attributes = new(id, context.DefaultAttributes, default);
            Positionable positionable = new(id, context.Position, default, 1f, default, context.Model);
            SkillOwner skillOwner = new(id, context.Skills);

            _killableRepository.Create(killable);
            _healthRepository.Create(health);
            _aligmentRepository.Create(aligment);
            _attributesRepository.Create(attributes);
            _resourceRepository.Create(new(id, ResourceId.Custom, 100, 0));
            _positionableRepository.Create(positionable);
            _actorRepository.Create(new(id, ActorState.Free, null));
            _skillOwnerRepository.Create(skillOwner);

            return positionable;
        }
    }
}
