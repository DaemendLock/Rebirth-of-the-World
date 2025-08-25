using System.Collections.Generic;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Services;
using Server.Combat.Domain.Skills;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Factories
{
    public class UnitModelFactory : IUnitModelFactory
    {
        private int _nextId = 0;

        private readonly IHealDamageApplicationService _healDamageApplictionService;
        private readonly IAttributeEvaluationService _attributeEvaluationService;

        private readonly IAttributesRepository _attributeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IHitboxesRepository _hitboxesRepository;
        private readonly IUnitControllerFactory _unitControllerFactory;
        private readonly ISkillFactory _skillFactory;

        public UnitModelFactory(IAttributesRepository attributeRepository, IHitboxesRepository hitboxesRepository, ISkillRepository skillRepository, IUnitControllerFactory unitControllerFactory, ISkillFactory skillFactory, IHealDamageApplicationService healDamageApplictionService)
        {
            _attributeRepository = attributeRepository;
            _hitboxesRepository = hitboxesRepository;
            _unitControllerFactory = unitControllerFactory;
            _skillRepository = skillRepository;
            _skillFactory = skillFactory;
            _healDamageApplictionService = healDamageApplictionService;
        }

        public Unit Create(IUnitModelFactory.UnitModelCreationData context)
        {
            EntityId id = new(GetNextId());

            HealthValue healthValue = new(context.CurrentHealth, true, context.MaxHealth);
            TransformValue transformValue = new()
            {
                Position = context.Position,
                Rotation = context.Rotation
            };

            IAttributeCollection<Attribute> attributes = _attributeRepository.Create(id, context.DefaultAttributes);
            _hitboxesRepository.Add(id, context.Hitboxes);
            List<Skill> skills = new();

            foreach (SkillId skillId in context.Skills)
            {
                Skill skill = _skillRepository.Get(skillId);

                if (skill == null)
                {
                    skill = _skillFactory.Create(skillId);
                    _skillRepository.Add(skill);
                }

                skills.Add(skill);
            }

            Killable killable = new();
            Transform transform = new();

            Unit result = new(id, context.UnitId, context.Team, killable, transform, _healDamageApplictionService);
            _unitControllerFactory.Create(result);

            return result;
        }

        private int GetNextId() => _nextId++;
    }
}
