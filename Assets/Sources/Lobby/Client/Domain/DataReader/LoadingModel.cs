using System;

using Client.Lobby.Domain.Common;

namespace Client.Lobby.Domain.Loading
{
    public interface LoadingSource
    {
        public void Load<T>(T model) where T : ILoadableModel;
    }

    public interface DelaiedData<T>
    {
        T GetValue();
    }

    internal class LoadingModel : IUpdateableModel
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

        public void Load<T>(T model) where T : ILoadableModel
        {
            Loading = true;
            _source.Load(model);
            Loading = false;
        }
    }
}
