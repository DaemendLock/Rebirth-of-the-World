using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ISpendResourceOutput
    {
        void Present(Resource resource);
    }
}
