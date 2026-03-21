using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;

namespace Combat.Local.Events
{
    public readonly ref struct CharacterCreatedInfo
    {
        public CharacterCreatedInfo(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }
    }

    public class CharacterCreatedHandler : ICreateUnitEventHandler
    {
        public delegate void Handler(CharacterCreatedInfo info);

        public event Handler Created;

        public void HandleEvent(EntityId id)
        {
            CharacterCreatedInfo info = new(id);
            Created?.Invoke(info);
        }
    }
}
