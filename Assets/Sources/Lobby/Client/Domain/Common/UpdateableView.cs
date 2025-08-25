using System;
using System.Threading.Tasks;

namespace Client.Lobby.Domain.Common
{
    public interface IUpdateableModel
    {
        event Action Updated;
    }

    public interface ILoadableModel
    {
        bool IsLoaded { get; }

        Task Load();
    }
}
