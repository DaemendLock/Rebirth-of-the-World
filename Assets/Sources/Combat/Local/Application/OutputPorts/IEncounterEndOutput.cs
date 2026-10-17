using Combat.Common.ValueObjects;

namespace Combat.Local.Application.OutputPorts
{
    public interface IEncounterEndOutput
    {
        void Present(EncounterState reason);
    }
}
