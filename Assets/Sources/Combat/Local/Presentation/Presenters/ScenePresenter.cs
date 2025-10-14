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
        private readonly IHitboxIniter _characterViewIniter;

        public ScenePresenter(ICharacterViewContainer container, ICharacterViewFactory factory, IHitboxIniter characterViewIniter)
        {
            _container = container;
            _factory = factory;
            _characterViewIniter = characterViewIniter;
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
            if (_container.TryGetValue(target, out Transform oldCharacterView))
            {
                oldCharacterView.gameObject.SetActive(false);
                Object.Destroy(oldCharacterView.gameObject);
            }

            _characterViewIniter.CreateHitboxes(target, view.transform);
            _container.Save(target, view.transform);
        }
    }
}
