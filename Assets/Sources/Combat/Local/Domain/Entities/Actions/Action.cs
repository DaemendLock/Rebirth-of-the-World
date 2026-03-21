using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public interface IActionScript
    {
        void OnStart(EntityId actor);
        void OnCancel(EntityId actor);
    }

    public readonly ref struct Action
    {
        public int Id { get; }
        public IActionScript Script { get; }
    }
}
