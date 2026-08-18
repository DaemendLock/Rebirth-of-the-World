using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.Primitives;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Ports.CharacterPorts
{
    public sealed class UnitLifecycleHandler
    {
        private readonly IUnitRuntimeRegistry _runtimeRegistry;

        public void Create(UnitId unitId) { }
        public void Remove(UnitId unitId) => _runtimeRegistry.Remove(unitId);
    }

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
