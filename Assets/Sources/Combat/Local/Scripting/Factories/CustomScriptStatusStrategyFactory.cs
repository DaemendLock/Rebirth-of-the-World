using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Scripting.Factories
{
    public class CustomScriptStatusStrategyFactory : IStatusRuntimeFactory
    {
        private readonly IStatusScriptTypeProvider _statusApiTypeProvider;
        private readonly AbilityApiAdapter _abilityApiAdapter;
        private readonly CharacterApiAdapter _characterApiAdapter;
        private readonly StatusApiAdapter _statusApiAdapter;

        public CustomScriptStatusStrategyFactory(IStatusScriptTypeProvider statusApiTypeProvider, AbilityApiAdapter abilityApiAdapter,
            CharacterApiAdapter characterApiAdapter, StatusApiAdapter statusApiAdapter)
        {
            _statusApiTypeProvider = statusApiTypeProvider;
            _characterApiAdapter = characterApiAdapter;
            _abilityApiAdapter = abilityApiAdapter;
            _statusApiAdapter = statusApiAdapter;

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusScript).IsAssignableFrom(value)))
            {
                _statusApiTypeProvider.Register(type);
            }
        }

        public bool CanHandle(StatusType name) =>
            _statusApiTypeProvider.TryGet(name, out Type type) && typeof(StatusScript).IsAssignableFrom(type);

        public StatusRuntime Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source)
        {
            if (TryCreateEmpty(name, out StatusScript script) == false)
            {
                return default;
            }

            script.Init(_statusApiAdapter.Adaptee(id, parent, source));
            return new(null, new OldStatusCapabilityProvider(script, _characterApiAdapter, _abilityApiAdapter));
        }

        public bool TryCreateEmpty(StatusType name, out StatusScript value)
        {
            if (_statusApiTypeProvider.TryGet(name, out Type targetType) == false)
            {
                value = default;
                return false;
            }

            value = (StatusScript)FormatterServices.GetUninitializedObject(targetType);
            return true;
        }
    }
}
