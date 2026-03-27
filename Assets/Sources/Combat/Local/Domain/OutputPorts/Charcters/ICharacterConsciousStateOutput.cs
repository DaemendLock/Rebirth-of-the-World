using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICharacterConsciousStateOutput
    {
        void Present(EntityId value, ConsciousState state);
    }
}
