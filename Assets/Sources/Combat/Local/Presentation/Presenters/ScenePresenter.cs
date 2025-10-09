using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;
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

        public void Present(Positionable value)
        {
            CharacterView view = _factory.Create(value.Id, value.ModelName);
            SetModel(value.Id, view);
        }

        public void SetTransform(Positionable positionable, Transform transform)
        {
            if (transform == null || transform.TryGetComponent(out CharacterView view) == false)
            {
                Present(positionable);
                return;
            }

            view.Id = positionable.Id;
            view.name = positionable.ModelName.ToString() + positionable.Id.ToString();
            SetModel(positionable.Id, view);
        }

        private void SetModel(EntityId target, CharacterView view)
        {
            _factory.Init(target, view);

            if (_container.TryGetValue(target, out Transform oldCharacterView))
            {
                oldCharacterView.gameObject.SetActive(false);
                Object.Destroy(oldCharacterView.gameObject);
            }

            _container.Save(target, view.transform);
        }
    }
}
