using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases.Scene;

using UnityEngine;

namespace Combat.Local.Events
{
    public readonly ref struct HitInfo
    {
        public HitInfo(EntityId source, EntityId target, HitboxType hitboxType, HurtboxType hurtboxType, Vector3 location, SkillId? handler)
        {
            Source = source;
            Target = target;
            HitboxType = hitboxType;
            HurtboxType = hurtboxType;
            Location = location;
            Handler = handler;
        }

        public EntityId Target { get; }

        public EntityId Source { get; }

        public HitboxType HitboxType { get; }

        public HurtboxType HurtboxType { get; }

        public Vector3 Location { get; }

        public SkillId? Handler { get; }
    }

    public class HitHandler : IHitEventHandler
    {
        public delegate void HandleHit(HitInfo info);
        private readonly IActorRepository _actorRepository;

        public HitHandler(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public event HandleHit Hitted;

        public void HandleEvent(Hitbox hitbox, Hurtbox hurtbox, Vector3 position)
        {
            var actor = _actorRepository.Get(hitbox.Owner);

            HitInfo hitInfo = new(hitbox.Owner, hurtbox.Owner, hitbox.Type, hurtbox.Type, position, actor.CurrentAction?.Skill);
            Hitted?.Invoke(hitInfo);
        }
    }
}
