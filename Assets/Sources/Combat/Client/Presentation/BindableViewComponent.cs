using UnityEngine;

using UtilsUnity.Patterns.View;

namespace Client.Combat.Presentation
{
    public abstract class BindableViewComponent<T> : MonoBehaviour, IBindableView<T>
    {
        private T _model;

        public T Model
        {
            get => _model;
            private set => _model = value;
        }

        public void Bind(T value)
        {
            T oldValue = Model;
            Model = value;
            OnModelUpdated(oldValue);
        }

        protected virtual void OnModelUpdated(T oldValue) { }
    }
}
