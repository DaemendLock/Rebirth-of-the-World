using Lobby.Common.Primitives;

using UnityEngine.SceneManagement;

namespace Combat.Local.Gateways.Repositories.Encounter
{
    public interface ILocationDataSource
    {
        Scene Load(LocationId location);
    }
}
