using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICharacterConsciousStateOutput
    {
        void Present(UnitId value, ConsciousState state);
    }
}
