using Combat.Local.Presentation;
using Combat.Local.Presentation.Units.ViewModels;

namespace Testing.Local.Temp.Factories
{
    public class HitboxViewModelFactory
    {
        private int _nextId;

        public HitboxViewModelFactory()
        {
            _nextId = 0;
        }

        public HitboxViewModel Create(Hitbox hitboxView)
        {
            HitboxViewModel result = hitboxView.gameObject.AddComponent<HitboxViewModel>();
            result.Id = new(_nextId++);
            result.Type = hitboxView.Type;
            return result;
        }
    }
}
