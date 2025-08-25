using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Skills;

namespace Testing.Local.Temp.Services
{
    public class HitHandler
    {
        private readonly EntityId _owner;
        private readonly IHitHandler _hitHandler;

        private readonly HashSet<EntityId> _hittedTargets;

        public HitHandler(EntityId owner, IHitHandler hitHandler)
        {
            _owner = owner;
            _hitHandler = hitHandler;
        }

        public bool HandleHit(HitRecord @event)
        {
            if (_hitHandler.CanHandle(default) == false)
            {
                return false;
            }

            if (@event.Target == null)
            {
                return _hitHandler.OnHit(@event);
            }

            if (_hittedTargets.Contains(@event.Target.Id))
            {
                return false;
            }

            _hittedTargets.Add(@event.Target.Id);

            return _hitHandler.OnHit(@event);
        }
    }
}
