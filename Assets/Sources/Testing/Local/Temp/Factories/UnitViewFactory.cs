using Combat.Common.ValueObjects;
using Combat.Local.Factories;
using Combat.Local.Infrastructure.Controllers;
using Combat.Local.Infrastructure.Presenters;
using Combat.Local.Presentation;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Implementations.Units;
using Combat.Local.Presentation.Units.ViewModels;

using Temp.Repositories.Implementations;

using UnityEngine;

namespace Testing.Local.Temp.Factories
{
    public class CharacterModelFactory
    {
        private readonly CharacterModelRepository _characterModelRepository;

        private readonly HitboxViewModelFactory _hitboxViewModelFactory = new();
        private readonly HurtboxViewModelFactory _hurtboxViewModelFactory = new();

        public CharacterModelFactory(CharacterModelRepository characterModelRepository, HitboxViewModelFactory hitboxViewModelFactory, HurtboxViewModelFactory hurtboxViewModelFactory)
        {
            _characterModelRepository = characterModelRepository;
            _hitboxViewModelFactory = hitboxViewModelFactory;
            _hurtboxViewModelFactory = hurtboxViewModelFactory;
        }

        public CharacterView Create(ModelName name, Transform parent)
        {
            GameObject prefab = _characterModelRepository.Get(name);

            if (prefab == null)
            {
                throw new System.InvalidOperationException();
            }

            GameObject model = Object.Instantiate(prefab, parent);

            Hitbox[] hitboxes = model.GetComponentsInChildren<Hitbox>();
            Hurtbox[] hurtboxes = model.GetComponentsInChildren<Hurtbox>();

            foreach (Hitbox hitboxData in hitboxes)
            {
                HitboxViewModel hitbox = _hitboxViewModelFactory.Create(hitboxData);
                Object.Destroy(hitboxData);
            }

            foreach (Hurtbox hurtboxData in hurtboxes)
            {
                HurtboxViewModel hurtbox = _hurtboxViewModelFactory.Create(hurtboxData);
                Object.Destroy(hurtboxData);
            }

            CharacterView view = model.GetComponent<CharacterView>() ?? model.AddComponent<CharacterView>();
            view.UnitName = name;
            return view;
        }
    }

    public class UnitViewFactory : IUnitViewFactory
    {
        private readonly CharacterModelRepository _characteModelRepository;

        private readonly HitController _hitController;

        private readonly CharacterModelFactory _characterModelFactory;
        private readonly HitboxViewModelFactory _hitboxViewModelFactory;
        private readonly HurtboxViewModelFactory _hurtboxViewModelFactory;

        public UnitViewFactory(HitController hitController, CharacterModelRepository characteModelRepository)
        {
            _hitController = hitController;
            _characteModelRepository = characteModelRepository;

            _hitboxViewModelFactory = new();
            _hurtboxViewModelFactory = new();
            _characterModelFactory = new(_characteModelRepository, _hitboxViewModelFactory, _hurtboxViewModelFactory);
        }

        public IUnitPresenter Create(IUnitViewFactory.UnitViewCreationInfo context)
        {
            Transform parent = context.Parent;
            UnitPresenter result = null;

            if (parent == null)
            {
                parent = new GameObject(context.EntityId.ToString()).transform;
            }

            parent.position = context.Position;
            CharacterView characterView = parent.GetComponentInChildren<CharacterView>();

            if (characterView != null)
            {
                Hitbox[] hitboxes = characterView.GetComponentsInChildren<Hitbox>();
                Hurtbox[] hurtboxes = characterView.GetComponentsInChildren<Hurtbox>();

                foreach (Hitbox hitboxData in hitboxes)
                {
                    _hitboxViewModelFactory.Create(hitboxData);
                    Object.Destroy(hitboxData);
                }

                foreach (Hurtbox hurtboxData in hurtboxes)
                {
                    _hurtboxViewModelFactory.Create(hurtboxData);
                    Object.Destroy(hurtboxData);
                }
            }
            else
            {
                characterView = _characterModelFactory.Create(context.ModelName, parent);
            }

            UnitViewModel viewModel = parent.gameObject.AddComponent<UnitViewModel>();
            viewModel.EntityId = context.EntityId;
            result = new UnitPresenter(viewModel, _hitController);
            result.SetModel(characterView);
            return result;
        }
    }
}
