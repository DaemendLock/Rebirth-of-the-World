using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICharacterCreateOutput
    {
        void Present(UnitId value);
    }

    public interface ICharacterRemoveOutput
    {
        void Present(UnitId value);
    }
}
