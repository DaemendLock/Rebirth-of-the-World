using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICreateUnitOutput
    {
        void Present(EntityId value);
    }
}
