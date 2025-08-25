using Client.Combat.Domain.Units;

namespace Client.Combat.Presentation.Units
{
    public interface IUnitPresenter
    {
        IUnit Model { get; }

        public bool OnGround { get; }
    }
}
