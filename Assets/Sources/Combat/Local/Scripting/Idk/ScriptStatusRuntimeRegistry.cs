using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;
using Combat.Local.Gateways.Repositories.Statuses;

using System;
using System.Collections.Generic;

namespace Combat.Local.Scripting.Idk
{
    public sealed class ScriptStatusRuntimeRegistry : IStatusRuntimeRegistry
    {
        private readonly Dictionary<StatusId, ApiScriptStatusPropertyContainer> _values;

        public void Create(StatusId id) => throw new NotImplementedException();

        public void Remove(StatusId id) => _values.Remove(id);

        public bool TryGet(StatusId id, out IStatusPropertyContainer container)
        {
            if (_values.TryGetValue(id, out var result) == false)
            {
                container = null;
                return false;
            }

            container = result;
            return true;
        }
    }
}
