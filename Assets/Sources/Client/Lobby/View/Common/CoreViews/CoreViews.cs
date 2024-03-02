using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;
using Client.Lobby.Core.Common;
using UnityEngine;

namespace Client.Lobby.View.Common.CoreViews
{
    public interface AccountView
    {
        void SetAccount(Account value);
    }

    public interface ICharacterView
    {
        void SetCharacter(Character value);
    }

    public abstract class CharacterView : MonoBehaviour, ICharacterView
    {
        public abstract void SetCharacter(Character value);
    }

    public abstract class BindableView<T> : MonoBehaviour where T : UpdateableModel
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

        protected abstract void OnModelUpdate();
    }

    public interface SpellView
    {
        void SetSpell(Spell value);
    }
}
