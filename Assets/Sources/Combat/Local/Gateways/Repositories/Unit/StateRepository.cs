using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class StateRepository : IStateRepository
    {
        private readonly Dictionary<EntityId, ConsciousState> _values;

        public StateRepository()
        {
            _values = new();
        }

        public void Create(CharacterState value) => _values.Add(value.Id, value.ConsciousState);

        public void Delete(EntityId id) => _values.Remove(id);

        public CharacterState Get(EntityId id) => new(id, _values[id], ActorState.None);

        public void Update(CharacterState value) => _values[value.Id] = value.ConsciousState;
    }
}
