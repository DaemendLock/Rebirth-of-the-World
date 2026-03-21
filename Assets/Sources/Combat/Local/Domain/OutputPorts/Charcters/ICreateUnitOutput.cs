using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using UnityEngine;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICreateUnitOutput
    {
        void Present(Positionable positionable, Transform parent = null);
    }

    public interface ICreateUnitEventHandler
    {
        void HandleEvent(EntityId id);
    }

    public interface IApplyStatusOutput
    {
        void Present(Status status);
    }

    public interface ISpendResourceOutput
    {
        void Present(Resource resource);
    }
}
