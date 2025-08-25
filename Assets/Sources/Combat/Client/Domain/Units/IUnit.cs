using Client.Combat.Domain.Units.Components;

namespace Client.Combat.Domain.Units
{
    public interface IUnit : IHealthOwner, IKillable, IActionOwner, IPositionOwner
    {
        int Id { get; }

        bool CanMove();
    }
}
