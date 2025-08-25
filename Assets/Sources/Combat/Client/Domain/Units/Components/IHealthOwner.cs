namespace Client.Combat.Domain.Units.Components
{
    public interface IHealthOwner
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }
}
