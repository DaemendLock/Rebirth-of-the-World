namespace Server.Combat.Domain.Statuses.StatusEffects
{
    public interface IAttackFailEffect
    {
        void OnEvade();
        void OnParry();
        void OnBlock();
    }
}
