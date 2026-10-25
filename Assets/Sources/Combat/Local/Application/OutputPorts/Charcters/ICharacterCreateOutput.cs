using Combat.Common.Primitives;

namespace Combat.Local.Application.OutputPorts
{
    public interface ICharacterCreateOutput
    {
        void Present(UnitId value);
    }

    public interface ICharacterRemoveOutput
    {
        void Present(UnitId value);
    }

    public interface ICharacterLocalTimeScaleOutput
    {
        void Present(UnitId target, float timeScale);
    }
}
