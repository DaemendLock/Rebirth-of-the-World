using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Objective
    {
        public readonly ObjectiveId Id { get; }
        //public readonly ObjectiveId? Parent { get; }
        public readonly string Name { get; }

        public ObjectiveState State { get; private set; }

        public Objective(ObjectiveId id, string name, ObjectiveState state = ObjectiveState.Running)
        {
            Id = id;
            Name = name;
            State = state;
        }

        public bool TryTransition(ObjectiveState target)
        {
            if (State != ObjectiveState.Running)
            {
                return false;
            }

            if (target == ObjectiveState.Running)
            {
                return false;
            }

            State = target;
            return true;
        }
    }
}
