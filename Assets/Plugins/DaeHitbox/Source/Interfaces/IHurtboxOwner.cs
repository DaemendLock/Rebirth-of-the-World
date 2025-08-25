namespace DaeHitbox
{
    public interface IHurtboxOwner<T> : IHurtboxOwner
    {
        T Owner { get; }
    }

    public interface IHurtboxOwner
    {
    }
}
