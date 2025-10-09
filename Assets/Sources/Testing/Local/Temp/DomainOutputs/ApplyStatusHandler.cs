using Combat.API;
using Combat.API.Controllers;
using Combat.Local.Data.Factories;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class ApplyStatusHandler : IApplyStatusEventHandler
    {
        private readonly StatusApiProvider _statusApiProvider;
        private readonly StatusApiFactory _statusApiFactory;

        public ApplyStatusHandler(StatusApiProvider statusApiProvider, StatusApiFactory statusApiFactory)
        {
            _statusApiProvider = statusApiProvider;
            _statusApiFactory = statusApiFactory;
        }

        public void HandleEvent(Status status)
        {
            StatusApi api = _statusApiFactory.Create(status.Id, status.Parent, status.Name, status.Source, status.Caster);

            if (api == null)
            {
                return;
            }

            if (api.TryGetProperty(out StatusScript handler) == false)
            {
                return;
            }

            _statusApiProvider.Register(api);
            handler.OnCreate();
        }
    }
}
