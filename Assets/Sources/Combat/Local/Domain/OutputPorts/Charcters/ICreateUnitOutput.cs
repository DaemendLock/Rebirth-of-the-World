using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICreateUnitOutput
    {
        void Present(Positionable positionable);
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
