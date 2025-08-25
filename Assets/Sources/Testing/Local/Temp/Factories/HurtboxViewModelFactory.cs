using Combat.Local.Presentation;
using Combat.Local.Presentation.Units.ViewModels;

namespace Testing.Local.Temp.Factories
{
    public class HurtboxViewModelFactory
    {
        private int _nextId;

        public HurtboxViewModelFactory()
        {
            _nextId = 0;
        }

        public HurtboxViewModel Create(Hurtbox hurtboxView)
        {
            HurtboxViewModel result = hurtboxView.gameObject.AddComponent<HurtboxViewModel>();
            result.Id = new(_nextId++);
            result.Type = hurtboxView.Type;
            return result;
        }
    }
}
