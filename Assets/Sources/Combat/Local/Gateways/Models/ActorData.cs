using Combat.Local.Domain.Entities;

namespace Combat.Local.Data.Models
{
    public readonly struct ActorData
    {
        public ActorData(ActorState state, Action action)
        {
            State = state;
            Action = action;
        }

        public ActorState State { get; }

        public Action Action { get; }
    }
}
