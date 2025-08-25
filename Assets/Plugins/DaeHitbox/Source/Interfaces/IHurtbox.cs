namespace DaeHitbox
{
    public enum HurtboxType
    {
        Head,
        Body,
        Legs
    }

    public interface IHurtbox
    {
        IHurtboxOwner Owner { get; }

        HurtboxType HurtboxType { get; }
    }
}
