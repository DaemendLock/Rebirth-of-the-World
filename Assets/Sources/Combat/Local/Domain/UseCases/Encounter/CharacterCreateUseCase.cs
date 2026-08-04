using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{
    public class CharacterCreateUseCase
    {
        private readonly UnitIdFactory _idFactory;

        private readonly IHealthRepository _healthRepository;
        private readonly IAligmentRepository _aligmentRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IAbilityRepository _abilityRepository;
        private readonly IAbilityFactory _abilityFactory;
        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        private readonly ICharacterCreateOutput _outputPort;

        public CharacterCreateUseCase(
            IHealthRepository healthRepository, IAligmentRepository aligmentRepository,
            IAttributesRepository attributesRepository, IResourceOwnerRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
            IPositionableRepository positionableRepository, IActorRepository actorRepository,
            ICharacterCreateOutput outputPort, IAbilityRepository skillRepository, IAbilityFactory skillFactory,
            ISkillLyfecycleHandler skillLyfecycleHandler, ICharacterUpdateRepository characterUpdateList,
            IStatusOwnerRepository statusOwnerRepository, IMovementEffectOwnerRepository movementEffectOwnerRepository,
            IHitboxOwnerRepository hitboxOwnerRepository, IHurtableRepository hurtableRepository)
        {
            _healthRepository = healthRepository;
            _aligmentRepository = aligmentRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;

            _idFactory = new(characterUpdateList);
            _abilityRepository = skillRepository;
            _abilityFactory = skillFactory;
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _characterUpdateList = characterUpdateList;
            _outputPort = outputPort;
            _statusOwnerRepository = statusOwnerRepository;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _hurtableRepository = hurtableRepository;
        }

        public void Execute(UnitId id, CreateCharacterDTO context)
        {
            Positionable positionable = Create(id, context);

            float currentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth;

            Health health = _healthRepository.Create(id, new(context.DefaultHealth, currentHealth));

            System.Span<SkillId> abilities = stackalloc SkillId[context.Skills.Length];
            int index = 0;

            foreach (SkillId skillId in context.Skills)
            {
                Ability ability = _abilityFactory.Create(skillId, id);
                _abilityRepository.Create(ability);
                _skillLyfecycleHandler.Give(new(id, skillId));
                abilities[index++] = ability.SkillId;
            }

            _skillOwnerRepository.Update(new(id, abilities));
            _outputPort.Present(id);
        }

        public UnitId Execute(CreateCharacterDTO context)
        {
            UnitId id = _idFactory.GetId();
            Execute(id, context);
            return id;
        }

        private Positionable Create(UnitId id, CreateCharacterDTO context)
        {
            if (_characterUpdateList.TryGet(id, out _))
            {
                throw new System.InvalidOperationException($"Character with Id {id} alreday exists");
            }

            Positionable positionable = new(id, context.Position, default, 1f, default, context.Model, Vector3.zero);
            _positionableRepository.Create(positionable);

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
            SkillOwner skillOwner = new(id, System.Span<SkillId>.Empty);

            _aligmentRepository.Create(aligment);
            _attributesRepository.Create(attributes);
            _resourceRepository.Create(new(id, context.DefaultResources));
            _actorRepository.Create(new(id, ActorState.None, null, ConsciousState.Alive, new(default, System.Numerics.Vector2.Zero)));
            _skillOwnerRepository.Create(skillOwner);
            _hitboxOwnerRepository.Create(id);
            _hurtableRepository.Create(id);
            _statusOwnerRepository.Create(new(id, System.Span<StatusId>.Empty));
            _characterUpdateList.Create(new(id, 1));
            _movementEffectOwnerRepository.Create(new(id, default));

            return positionable;
        }
    }
}
