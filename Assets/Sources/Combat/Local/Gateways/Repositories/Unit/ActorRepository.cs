using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class ActorRepository : IActorRepository
    {
        private readonly IStatusApiDataSource _apiDataSource;
        private readonly Dictionary<EntityId, ActorData> _values;

        public ActorRepository(IStatusApiDataSource apiDataSource)
        {
            _values = new();
            _apiDataSource = apiDataSource;
        }

        public void Create(Actor value) => _values[value.Id] = new(value.State, value.CurrentAction);

        public void Update(Actor value) => _values[value.Id] = new(value.State, value.CurrentAction);

        public Actor Get(EntityId id)
        {
            ActorData data = _values[id];
            ActorState state = data.State;

            if (_apiDataSource.RestrictMovement(id))
            {
                state |= ActorState.Rooted;
            }

            return new(id, state, data.Action);
        }

        public void Delete(EntityId id) => _values.Remove(id);

        public ICollection<EntityId> GetAll() => _values.Keys;

        private bool CanMove(EntityId entityId)
        {


            return true;
        }
    }
}
