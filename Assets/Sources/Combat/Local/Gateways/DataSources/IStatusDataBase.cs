using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Gateways.DataSources
{
    public interface IStatusDataBase
    {
        void Register(Type type);
        bool TryGet(StatusType key, out Type type);
    }
}
