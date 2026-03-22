using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases.Scene
{
    public class HandleHitsUseCase
    {
        private readonly IHitRecordRepository _hitRecordRepository;
        private readonly IHitboxRepository _hitboxRepository;
        private readonly IHurtableRepository _hurtboxRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ISkillRepository _skillRepository;

        public HandleHitsUseCase(IHitRecordRepository hitRecordRepository, IHitboxRepository hitboxRepository, IHurtableRepository hurtboxRepository, IActorRepository actorRepository, ISkillRepository skillRepository)
        {
            _hitRecordRepository = hitRecordRepository;
            _hitboxRepository = hitboxRepository;
            _hurtboxRepository = hurtboxRepository;
            _actorRepository = actorRepository;
            _skillRepository = skillRepository;
        }

        public void Execute()
        {
            while (_hitRecordRepository.TryPop(out HitRecord value))
            {
                HandleRecord(value);
            }
        }

        private void HandleRecord(HitRecord value)
        {
            Hitbox hitbox = _hitboxRepository.Get(value.HitboxId);
            Hurtbox hurtbox = _hurtboxRepository.Get(value.HurtboxId);
            Actor actor = _actorRepository.Get(hitbox.Owner);

            if (actor.CurrentAction == null)
            {
                return;
            }

            if (actor.CurrentAction.CurrentState != Common.ValueObjects.ActionState.Active)
            {
                return;
            }

            if (actor.CurrentAction.HittedTargets.Contains(hurtbox.Owner))
            {
                return;
            }

            actor.CurrentAction.HittedTargets.Add(hurtbox.Owner);

            Skill handler = _skillRepository.Get(new(actor.CurrentAction.Id.Value), hitbox.Owner);

            if (handler.TryGetEffect(out SkillHitEffect hitEffect))
            {
                hitEffect.HandleHit(hitbox, hurtbox, value.Location);
            }
        }
    }
}
