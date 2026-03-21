using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Factories;

using UnityEngine;

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

        public void Present(Positionable value, Transform transform)
        {
            if (transform == null || transform.TryGetComponent(out CharacterView view) == false)
            {
                view = _factory.Create(value.Id, value.ModelName);
            }

            view.Id = value.Id;
            view.name = value.ModelName.ToString() + value.Id.ToString();
            _factory.Init(view);

            if (_container.TryGetValue(value.Id, out Transform oldCharacterView))
            {
                oldCharacterView.gameObject.SetActive(false);
                Object.Destroy(oldCharacterView.gameObject);
            }

            _container.Save(value.Id, view.transform);
        }
    }
}
