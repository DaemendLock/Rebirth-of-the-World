using Combat.Common.Primitives;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;
using System;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public class UnitCreateUseCase
    {
        private readonly UnitIdFactory _idFactory;

        private readonly IHealthRepository _healthRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;
        private readonly IHurtableRepository _hurtableRepository;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly IScaleEffectOwnerRepository _scaleEffectOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        private readonly ICharacterCreateOutput _outputPort;

        public UnitCreateUseCase(
            IHealthRepository healthRepository, IAttributesRepository attributesRepository,
            IResourceOwnerRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
            IPositionableRepository positionableRepository, IActorRepository actorRepository,
            ICharacterCreateOutput outputPort,
            ISkillLyfecycleHandler skillLyfecycleHandler, ICharacterUpdateRepository characterUpdateList,
            IStatusOwnerRepository statusOwnerRepository, IMovementEffectOwnerRepository movementEffectOwnerRepository,
            IHitboxOwnerRepository hitboxOwnerRepository, IHurtableRepository hurtableRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository)
        {
            _healthRepository = healthRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;

            _idFactory = new(characterUpdateList);
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _characterUpdateList = characterUpdateList;
            _outputPort = outputPort;
            _statusOwnerRepository = statusOwnerRepository;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _hurtableRepository = hurtableRepository;
            _scaleEffectOwnerRepository = scaleEffectOwnerRepository;
        }

        public void Execute(UnitId id, UnitCreationInfo context)
        {
            if (_characterUpdateList.TryGet(id, out _))
            {
                throw new System.InvalidOperationException($"Character with Id {id} alreday exists");
            }

            _positionableRepository.Create(CreatePositionable(id, context));
            _attributesRepository.Create(CreateAttributesOwner(id, context));

            UnsortedCreate(id, context);

            float currentHealth = context.InitialHealth < 0 ? context.DefaultHealth : context.InitialHealth;
            _healthRepository.Create(new(id, currentHealth, context.DefaultHealth));

            SkillOwner skillOwner = CreateSkillOwner(id, context);
            _skillOwnerRepository.Create(skillOwner);
            _statusOwnerRepository.Create(CreateStatusOwner(id));

            foreach (SkillId skillId in skillOwner.Skills)
            {
                _skillLyfecycleHandler.Give(new(id, skillId));
            }

            _outputPort.Present(id);
        }

        public UnitId Execute(UnitCreationInfo context)
        {
            UnitId id = _idFactory.GetId();
            Execute(id, context);
            return id;
        }

        private Positionable CreatePositionable(UnitId id, UnitCreationInfo context) => new(id, context.Position, default, 1f, default, context.Model, Vector3.zero, context.Team);

        private SkillOwner CreateSkillOwner(UnitId id, UnitCreationInfo context) => new(id, context.Skills, System.Span<SkillCooldown>.Empty);

        private AttributesOwner CreateAttributesOwner(UnitId id, UnitCreationInfo context)
        {
            AttributeValue[] attributeValue = new AttributeValue[AttributesOwner.AttributeCount];

            for (int i = 0; i < AttributesOwner.AttributeCount; i++)
            {
                if (i < context.DefaultAttributes.Length)
                {
                    attributeValue[i] = context.DefaultAttributes[i];
                }
            }

            return new(id, attributeValue);
        }

        private void UnsortedCreate(UnitId id, UnitCreationInfo context)
        {
            Span<ResourceValue> resourceValues = stackalloc ResourceValue[context.DefaultResources.Length];
            context.DefaultResources.CopyTo(resourceValues);

            _resourceRepository.Create(new(id, resourceValues));
            _actorRepository.Create(new(id, ActorState.None, null, ConsciousState.Alive, new(default, System.Numerics.Vector2.Zero)));
            _hitboxOwnerRepository.Create(id);
            _hurtableRepository.Create(id);
            _characterUpdateList.Create(new(id, 1));
            _movementEffectOwnerRepository.Create(new(id, default));
            _scaleEffectOwnerRepository.Create(new(id, Span<ScaleOverTimeEffect>.Empty));
        }

        private StatusOwner CreateStatusOwner(UnitId id) => new(id, Span<StatusId>.Empty);
    }
}
