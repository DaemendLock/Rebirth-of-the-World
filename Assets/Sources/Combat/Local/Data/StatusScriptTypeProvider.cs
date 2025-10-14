using Combat.Api.Controllers.Factories;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Combat.Local.Data.Databases
{
    public class StatusScriptTypeProvider : IStatusScriptTypeProvider
    {
        private readonly Type _scriptType;
        private readonly Dictionary<StatusName, Type> _typesByName;

        public StatusScriptTypeProvider(Type scriptType)
        {
            _scriptType = scriptType;
            _typesByName = new();
        }

        public void Register(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return;
            }

            if (_scriptType.IsAssignableFrom(type) == false)
            {
                UnityEngine.Debug.LogError($"Can't register status: not implementing {_scriptType}");
                return;
            }

            StatusScriptNameAttribute skillScriptNameAttribute = type.GetCustomAttribute<StatusScriptNameAttribute>();

            if (skillScriptNameAttribute == null)
            {
                return;
            }

            _typesByName[skillScriptNameAttribute.Name] = type;
        }

        public bool TryGet(StatusName key, out Type type) => _typesByName.TryGetValue(key, out type);
    }
}
