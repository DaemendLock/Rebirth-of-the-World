using Client.Combat.Domain.Actions;

namespace Client.Combat.Domain.Units.Components
{
    public interface IActionOwner
    {
        IActionHandler ActiveAction { get; }
    }
}
