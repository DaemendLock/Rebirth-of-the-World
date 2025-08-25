using Client.Lobby.Domain.Characters;
using Client.Lobby.Domain.Common;

using UnityEngine;

using UtilsUnity.Patterns.View;

namespace Client.Lobby.View.Common.CoreViews
{
    public abstract class CharacterView : MonoBehaviour, IBindableView<Character>
    {
        public abstract void Bind(Character value);
    }

    public abstract class BindableView<T> : MonoBehaviour, IBindableView<T> where T : IUpdateableModel
    {
        private T _model;

        public T Model
        {
            get => _model;
            protected set
            {
                if (enabled == false)
                {
                    _model = value;
                    return;
                }

                OnDisable();
                _model = value;
                OnEnable();
            }
        }

        private void OnEnable()
        {
            if (Model == null)
            {
                return;
            }

            OnModelUpdate();
            Model.Updated += OnModelUpdate;
        }

        private void OnDisable()
        {
            if (Model == null)
            {
                return;
            }

            Model.Updated -= OnModelUpdate;
        }

        public void Bind(T value) => Model = value;

        protected abstract void OnModelUpdate();
    }

    public interface SpellView
    {
        void SetSpell(Spell value);
    }
}
