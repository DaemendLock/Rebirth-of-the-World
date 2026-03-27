using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IApplyStatusOutput
    {
        void Present(Status status);
    }
}
