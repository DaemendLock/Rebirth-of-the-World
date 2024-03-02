using System;

using Client.Lobby.Core.Common;

namespace Client.Lobby.Core.Loading
{
    public interface LoadingSource
    {
        public void Load<T>(T model) where T : LoadableModel;
    }

    public interface DelaiedData<T>
    {
        T GetValue();
    }

    internal class LoadingModel : UpdateableModel
    {
        public event Action Updated;

        private readonly LoadingSource _source;
        private bool _loading;

        public bool Loading
        {
            get => _loading;
            private set
            {
                _loading = value;
                Updated?.Invoke();
            }
        }

        public void Load<T>(T model) where T : LoadableModel
        {
            Loading = true;
            _source.Load(model);
            Loading = false;
        }
    }
}
