using Combat.API.Adapters;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Gateways.Repositories.Statuses;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.IDK;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Scripting.Factories
{
    public class CustomScriptStatusStrategyFactory : IStatusPropertyContainerFactory
    {
        private readonly IStatusScriptTypeProvider _statusApiTypeProvider;
        private readonly AbilityApiAdapter _abilityApiAdapter;
        private readonly CharacterApiAdapter _characterApiAdapter;
        private readonly IStatusApiAdapter _statusApiAdapter;

        public CustomScriptStatusStrategyFactory(IStatusScriptTypeProvider statusApiTypeProvider, AbilityApiAdapter abilityApiAdapter, CharacterApiAdapter characterApiAdapter, IStatusApiAdapter statusApiAdapter)
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

        public bool CanHandle(StatusType name) => _statusApiTypeProvider.TryGet(name, out Type _);

        public IStatusPropertyContainer Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source)
        {
            if (TryCreateEmpty(name, out StatusScript script) == false)
            {
                return null;
            }

            script.Init(_statusApiAdapter.Adaptee(id, parent, source));
            return new ApiScriptStatusPropertyContainer(script, _characterApiAdapter, _abilityApiAdapter);
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
