using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public ref struct Objective
    {
        public readonly ObjectiveId Id { get; }
        //public readonly ObjectiveId? Parent { get; }
        public readonly string Name { get; }
        public bool IsCompleted { get; set; }

        public Objective(ObjectiveId id, string name, bool isCompleted)
        {
            Id = id;
            Name = name;
            IsCompleted = isCompleted;
        }
    }
}
