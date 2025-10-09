using Combat.API;
using Combat.API.Controllers;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class RemoveStatusHandler : IRemoveStatusEventHandler
    {
        private readonly StatusApiProvider _statusApiProvider;

        public RemoveStatusHandler(StatusApiProvider statusApiProvider)
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

            handler.OnRemove();
            _statusApiProvider.Delete(statusId);
        }
    }
}
