using Combat.API;
using Combat.API.Controllers;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class StatusExpireHandler : IStatusExpiredEventHandler
    {
        private readonly StatusApiProvider _statusApiProvider;

        public StatusExpireHandler(StatusApiProvider statusApiProvider)
        {
            _statusApiProvider = statusApiProvider;
        }

        public void HandleEvent(StatusId statusId)
        {
            StatusApi api = _statusApiProvider.Get(statusId);

            if (api == null)
            {
                return;
            }

            if (api.TryGetProperty(out StatusScript handler) == false)
            {
                return;
            }

            handler.OnExpire();
        }
    }
}
