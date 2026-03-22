using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Presentation.Components;

namespace Combat.Local.Presentation.Presenters
{
    public class ScenePresenter : ICreateUnitOutput
    {
        private readonly ICharacterViewContainer _container;

        public ScenePresenter(ICharacterViewContainer container)
        {
            _container = container;
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
        }
    }
}
