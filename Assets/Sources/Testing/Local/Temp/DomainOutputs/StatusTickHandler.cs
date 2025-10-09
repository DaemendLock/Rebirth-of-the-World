using Combat.API;
using Combat.API.Controllers;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class StatusTickHandler : IStatusTickEventHandler
    {
        private readonly StatusApiProvider _statusApiProvider;

        public StatusTickHandler(StatusApiProvider statusApiProvider)
        {
            _statusApiProvider = statusApiProvider ?? throw new System.ArgumentNullException(nameof(statusApiProvider));
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

            handler.OnTick();
        }
    }
}
