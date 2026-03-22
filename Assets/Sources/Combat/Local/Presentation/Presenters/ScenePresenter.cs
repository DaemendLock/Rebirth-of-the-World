using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Factories;

namespace Combat.Local.Presentation.Presenters
{
    public class ScenePresenter : ICreateUnitOutput
    {
        private readonly ICharacterViewContainer _container;
        private readonly ICharacterViewFactory _factory;

        public ScenePresenter(ICharacterViewContainer container, ICharacterViewFactory factory)
        {
            _container = container;
            _factory = factory;
        }

        public void Present(Positionable value)
        {
            if (_container.TryGetValue(value.Id, out var transform) == false)
            {
                return;
            }

            CharacterView view = transform.GetComponent<CharacterView>() ?? transform.gameObject.AddComponent<CharacterView>();
            view.Id = value.Id;
            view.name = value.ModelName.ToString() + value.Id.ToString();
            _factory.Init(view);
        }
    }
}
