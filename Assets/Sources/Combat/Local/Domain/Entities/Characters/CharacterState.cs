using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct CharacterState
    {
        public CharacterState(EntityId id)
        {
            Id = id;
            ConsciousState = ConsciousState.Alive;
            ActorState = ActorState.None;
        }

        public CharacterState(EntityId id, ConsciousState consciousState, ActorState actorState) : this(id)
        {
            ConsciousState = consciousState;
            ActorState = actorState;
        }

        public EntityId Id { get; }

        public ConsciousState ConsciousState { get; set; }

        public ActorState ActorState { get; set; }
    }
}
