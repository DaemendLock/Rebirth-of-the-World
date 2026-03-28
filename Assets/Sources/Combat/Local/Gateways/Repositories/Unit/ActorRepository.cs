using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class ActorRepository : IActorRepository
    {
        private readonly Dictionary<EntityId, ActorData> _values;

        public ActorRepository()
        {
            _values = new();
        }

        public void Create(Actor value) => _values[value.Id] = new(value.State, value.CurrentAction);

        public void Update(Actor value) => _values[value.Id] = new(value.State, value.CurrentAction);

        public Actor Get(EntityId id)
        {
            if (_values.TryGetValue(id, out ActorData data) == false)
            {
                return default;
            }

            ActorState state = data.State;

            return new(id, state, data.Action);
        }

        public void Delete(EntityId id) => _values.Remove(id);

        public ICollection<EntityId> GetAll() => _values.Keys;
    }
}
