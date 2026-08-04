using Combat.API.Adapters;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;
using Combat.Local.Gateways.Repositories.Statuses;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.Idk;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Scripting.Factories
{
    public class CustomScriptStatusStrategyFactory : IStatusPropertyContainerFactory
    {
        private readonly IStatusScriptTypeProvider _statusApiTypeProvider;
        private readonly IStatusApiAdapter _statusApiFactory;

        public CustomScriptStatusStrategyFactory(IStatusApiAdapter statusApiFactory, IStatusScriptTypeProvider statusApiTypeProvider)
        {

            _statusApiTypeProvider = statusApiTypeProvider;
            _statusApiFactory = statusApiFactory;

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

            script.Init(_statusApiFactory.Adaptee(id, parent, source));
            return new ApiScriptStatusPropertyContainer(script);
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
