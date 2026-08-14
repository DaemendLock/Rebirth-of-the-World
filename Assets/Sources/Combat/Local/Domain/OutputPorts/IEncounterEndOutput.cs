using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IEncounterEndOutput
    {
        void Present(EncounterState reason);
    }
}
