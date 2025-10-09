using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases.Scene;

using UnityEngine;

namespace Testing.Local.Temp.DomainOutputs
{
    public class HitHandler : IHitEventHandler
    {
        private readonly UnitApiProvider _unitApiRepository;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly IActorRepository _actorRepository;

        public HitHandler(UnitApiProvider unitApiRepository, SkillApiProvider skillApiProvider, IActorRepository actorRepository)
        {
            _unitApiRepository = unitApiRepository;

            _skillApiProvider = skillApiProvider;
            _actorRepository = actorRepository;
        }

        public void HandleEvent(Hitbox hitbox, Hurtbox hurtbox, Vector3 position)
        {
            Combat.API.DTO.HitRecord @event = CreateHitRecord(hitbox, hurtbox, position);

            Handle(hitbox.Owner, @event);
        }

        private Combat.API.DTO.HitRecord CreateHitRecord(Hitbox hitbox, Hurtbox hurtbox, Vector3 position)
        {
            Unit hitboxOwner = _unitApiRepository.Get(hitbox.Owner);
            Unit hurtboxOwner = _unitApiRepository.Get(hurtbox.Owner);
            return new(hitboxOwner, hitbox.Type, hurtboxOwner, hurtbox.Type, position);
        }

        private void Handle(EntityId target, Combat.API.DTO.HitRecord @event)
        {
            var actor = _actorRepository.Get(target);

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.HittedTargets.Contains(@event.Target.Id))
            {
                return;
            }

            actor.CurrentAction.HittedTargets.Add(@event.Target.Id);
            SkillId skillId = actor.CurrentAction.Skill;

            SkillApi skill = _skillApiProvider.Get(skillId, target);

            if (skill.TryGetProperty(out IHitHandler handler) == false)
            {
                return;
            }

            handler.OnHit(@event);
        }
    }
}
