using Combat.Common;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;

namespace Combat.Local.Application.OutputPorts
{
    public interface ICharacterDeathHandler
    {
        void Handle(KillRecord record);
    }

    public interface ICharacterConsciousStateOutput
    {
        void Present(UnitId value, ConsciousState state);
    }
}
