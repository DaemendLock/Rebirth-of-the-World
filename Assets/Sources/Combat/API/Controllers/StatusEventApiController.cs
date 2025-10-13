using Combat.API.Controllers.Factories;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Events;

namespace Combat.API.Controllers
{
    public class StatusEventApiController
    {
        private readonly StatusApiProvider _statusApiProvider;
        private readonly IStatusApiFactory _statusApiFactory;

        public StatusEventApiController(StatusApiProvider statusApiProvider, IStatusApiFactory statusApiFactory)
        {
            _statusApiProvider = statusApiProvider;
            _statusApiFactory = statusApiFactory;
        }

        public void HandleCreate(StatusCreateInfo info)
        {
            StatusApi api = _statusApiFactory.Create(info.StatusId, info.ParentId, info.StatusName, info.Source, info.Caster);

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

        public void HandleTick(StatusId target)
        {
            StatusApi api = _statusApiProvider.Get(target);

            if (api is null)
            {
                return;
            }

            if (api.TryGetProperty(out StatusScript script) == false)
            {
                return;
            }

            script.OnTick();
        }

        public void HandleExpire(StatusId target)
        {
            StatusApi api = _statusApiProvider.Get(target);

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

        public void HandleRemove(StatusId target)
        {
            StatusApi api = _statusApiProvider.Get(target);

            if (api == null)
            {
                return;
            }

            if (api.TryGetProperty(out StatusScript handler) == false)
            {
                return;
            }

            handler.OnRemove();
            _statusApiProvider.Delete(target);
        }
    }
}
