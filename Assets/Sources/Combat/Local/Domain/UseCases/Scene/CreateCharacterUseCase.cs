using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class CreateCharacterUseCase
    {
        private readonly UnitIdFactory _idFactory;

        private readonly HealthFactory _healthFactory;
        private readonly CharacterStateFactory _stateFactory;
        private readonly IAligmentRepository _aligmentRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillFactory _skillFactory;
        private readonly ICharacterUpdateList _characterUpdateList;

        private readonly ICreateUnitOutput _outputPort;

        public CreateCharacterUseCase(
            IHealthRepository healthRepository, IStateRepository killableRepository, IAligmentRepository aligmentRepository,
            IAttributesRepository attributesRepository, IResourceRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
            IPositionableRepository positionableRepository, IActorRepository actorRepository,
            ICreateUnitOutput outputPort, ISkillRepository skillRepository, ISkillFactory skillFactory, ICharacterUpdateList characterUpdateList)
        {
            _healthFactory = new(healthRepository);
            _stateFactory = new(killableRepository);
            _aligmentRepository = aligmentRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _outputPort = outputPort;

            _idFactory = new();
            _skillRepository = skillRepository;
            _skillFactory = skillFactory;
            _characterUpdateList = characterUpdateList;
        }

        public void Execute(CreateCharacterDTO context, Transform parent = null)
        {
            EntityId id = _idFactory.GetId();

            _stateFactory.Create(id);
            _healthFactory.Create(id, context.DefaultHealth, context.InitialHealth);

            Positionable positionable = Create(id, context, parent);

            _outputPort.Present(positionable);
            _positionableRepository.Update(positionable);
            SkillOwner skillOwner = _skillOwnerRepository.Get(positionable.Id);

            _characterUpdateList.Create(new(id, 1));

            foreach (SkillId skillId in skillOwner.GetAll())
            {
                Skill skill = _skillFactory.Create(skillId, id);
                _skillRepository.Create(skill);
            }
        }

        private Positionable Create(EntityId id, CreateCharacterDTO context, Transform parent)
        {
            Aligment aligment = new(id, context.Team);

            AttributeValue[] attributeValue = new AttributeValue[AttributesOwner.AttributeCount];

            for (int i = 0; i < AttributesOwner.AttributeCount; i++)
            {
                if (i < context.DefaultAttributes.Length)
                {
                    attributeValue[i] = context.DefaultAttributes[i];
                }
            }

            AttributesOwner attributes = new(id, context.DefaultAttributes);
            Positionable positionable = new(id, context.Position, default, 1f, default, context.Model);
            SkillOwner skillOwner = new(id, context.Skills);

            _aligmentRepository.Create(aligment);
            _attributesRepository.Create(attributes);
            _resourceRepository.Create(new(id, ResourceId.Custom, 100, 0));
            _positionableRepository.Create(positionable, parent);
            _actorRepository.Create(new(id, ActorState.None, null));
            _skillOwnerRepository.Create(skillOwner);

            return positionable;
        }

        private readonly struct CharacterStateFactory
        {
            private readonly IStateRepository _stateRepository;

            public CharacterStateFactory(IStateRepository stateRepository)
            {
                _stateRepository = stateRepository;
            }

            public CharacterState Create(EntityId id)
            {
                CharacterState state = new(id);
                _stateRepository.Create(state);
                return state;
            }
        }

        private readonly struct HealthFactory
        {
            private readonly IHealthRepository _healthRepository;

            public HealthFactory(IHealthRepository healthRepository)
            {
                _healthRepository = healthRepository;
            }

            public Health Create(EntityId id, float defaultValue, float initialValue)
            {
                Health health = new(id, defaultValue)
                {
                    CurrentHealth = initialValue < 0 ? defaultValue : initialValue,
                };
                _healthRepository.Create(health);

                return health;
            }
        }
    }
}
