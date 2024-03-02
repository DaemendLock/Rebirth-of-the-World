using System.Threading.Tasks;

namespace Sources.Client.Lobby.Infrastructure.Controllers
{
    public class ServerLoadingController
    {
        private readonly object? _source;

        public async Task<object?> GetServerData()
        {
            return _source;
        }
    }
}
