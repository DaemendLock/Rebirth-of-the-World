using Lobby.Common.Primitives;

using UnityEngine.SceneManagement;

namespace Combat.Local.Gateways.DataSources
{
    public interface ILocationDataSource
    {
        Scene Load(LocationId location);
    }
}
