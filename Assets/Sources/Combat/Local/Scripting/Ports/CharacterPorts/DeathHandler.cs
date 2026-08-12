using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Scripting.Ports.CharacterPorts
{
    public sealed class DeathHandler : ICharacterDeathHandler
    {
        private readonly IEventContext _eventContext;

        public DeathHandler(IEventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public void Handle(KillRecord record)
        {
            _eventContext.Publish<UnitDiedEventData>(new(new(record.Victim)));
        }
    }
}
