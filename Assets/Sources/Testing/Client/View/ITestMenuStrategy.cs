namespace Client.Testing.View
{
    public interface ITestMenuStrategy
    {
        void Kill();
        void Resurrect();
        void TakeDamage();
        void HalfHealth();
        void HealHealth();
        void Select();
    }
}
