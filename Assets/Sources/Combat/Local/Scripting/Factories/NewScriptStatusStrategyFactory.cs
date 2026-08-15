using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public sealed class NewScriptStatusStrategyFactory : IStatusRuntimeFactory
    {
        private readonly UnitNewAdapter _unitAdapter;
        private readonly IStatusDynamicMemoryRepository _memoryRepository;
        private readonly IEventContext _eventContext;
        private readonly StatusFacade _statusFacade;

        public NewScriptStatusStrategyFactory(UnitNewAdapter unitAdapter, IStatusDynamicMemoryRepository memoryRepository,
            IEventContext eventContext, StatusFacade statusFacade)
        {
            _unitAdapter = unitAdapter;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
            _statusFacade = statusFacade;
        }

        public bool CanHandle(StatusType name) => true;

        public StatusRuntime Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source)
        {
            DomainStatusContext context = new(id, _memoryRepository, _eventContext, _statusFacade);
            NewScriptStatusCapabilityProvider container = new(_unitAdapter.Adaptee(parent), null);
            return new(context, container);
        }
    }
}
